using Finances.Application.Abstractions.Repositories;
using Finances.Domain.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text;
using Worker.Parsers;

namespace Worker.Jobs; 

[DisallowConcurrentExecution]
internal class CurrencyUpdateJob(
    IHttpClientFactory httpClientFactory,
    ILogger<CurrencyUpdateJob> logger,
    IConfiguration configuration,
    ICurrenciesRepository currenciesRepository //Возможно стоит использовать тоже CQRS, т.к. мы работаем с одной БД
    ) : IJob
{
    public async Task Execute(IJobExecutionContext jobContext)
    {
        try
        { 
            logger.LogInformation("Запуск обновления курсов валют");

            var client = httpClientFactory.CreateClient();
            var response = await client.GetAsync(configuration.GetValue<string>("CbrUrl"), jobContext.CancellationToken);
            response.EnsureSuccessStatusCode();
            var bytes = await response.Content.ReadAsByteArrayAsync(jobContext.CancellationToken);
            var xmlContent = Encoding.GetEncoding("windows-1251").GetString(bytes);
            var newCurrencies = XmlParser.Parse(xmlContent);

            var existingCurrencies = await currenciesRepository.GetAllAsync();

            var currenciesDict = existingCurrencies.ToDictionary(c => c.Id, c => c);

            var currenciesToUpdate = new List<Currency>();
            var currenciesToAdd = new List<Currency>();

            foreach (var newCurrency in newCurrencies)
            {
                if (currenciesDict.TryGetValue(newCurrency.Id, out var existingCurrency))
                {
                    existingCurrency.Rate = newCurrency.Rate;
                    existingCurrency.Name = newCurrency.Name;
                    currenciesToUpdate.Add(existingCurrency);
                }
                else
                {
                    currenciesToAdd.Add(newCurrency);
                }
            }

            if (currenciesToUpdate.Count > 0)
            {
                currenciesRepository.UpdateRange(currenciesToUpdate);
                logger.LogInformation("Обновлено {Count} курсов.", currenciesToUpdate.Count);
            }

            if (currenciesToAdd.Count > 0)
            {
                await currenciesRepository.AddRangeAsync(currenciesToAdd, jobContext.CancellationToken);
                logger.LogInformation("Добавлено {Count} новых валют.", currenciesToAdd.Count);
            }

            await currenciesRepository.SaveChangesAsync(jobContext.CancellationToken);

            logger.LogInformation("Обновление курсов успешно завершено.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при выполнении Job'ы по обновлению курсов валют.");
        }
    }
}