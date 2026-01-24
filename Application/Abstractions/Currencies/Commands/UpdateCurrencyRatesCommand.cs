using Finances.Application.Abstractions.Shared;
using Finances.Domain.Db.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Finances.Application.Abstractions.Currencies.Commands;

public record UpdateCurrencyRatesCommand : IRequest;

public class UpdateCurrencyRatesCommandHandler(
    ICbrClient cbrClient,
    ICurrenciesRepository repository,
    IStateSaveChanges stateSaveChange,
    ILogger<UpdateCurrencyRatesCommandHandler> logger) : IRequestHandler<UpdateCurrencyRatesCommand>
{
    public async Task Handle(UpdateCurrencyRatesCommand request, CancellationToken ct)
    {
        logger.LogInformation("Start update currencies...");
        var newCurrencies = await cbrClient.GetCurrencies(ct);

        var existingCurrencies = await repository.GetAll(ct);

        var currenciesDict = existingCurrencies.ToDictionary(c => c.Name, c => c);

        var currenciesToUpdate = new List<Currency>();
        var currenciesToAdd = new List<Currency>();

        foreach (var newCurrency in newCurrencies)
        {
            if (currenciesDict.TryGetValue(newCurrency.Name, out var existingCurrency))
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
            repository.UpdateRange(currenciesToUpdate);
            logger.LogInformation("Обновлено {Count} курсов.", currenciesToUpdate.Count);
        }

        if (currenciesToAdd.Count > 0)
        {
            await repository.AddRange(currenciesToAdd, ct);
            logger.LogInformation("Добавлено {Count} новых валют.", currenciesToAdd.Count);
        }

        await stateSaveChange.SaveChangesAsync();

        logger.LogInformation("Валюты успешно обновлены.");
    }
}