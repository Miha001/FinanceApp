using Finances.Application.Abstractions.Currencies;
using Finances.Application.Abstractions.Shared;
using Finances.Domain.Db.Entities;
using Finances.Worker.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Finances.Worker.Currencies.Commands;

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

        var cbrRates = await cbrClient.GetCurrencies(ct);

        var existingCurrencies = await repository.GetAll(ct);
        var currenciesDict = existingCurrencies.ToDictionary(c => c.Name, c => c);

        var currenciesToAdd = new List<Currency>();
        int updatedCount = 0;

        foreach (var cbrRate in cbrRates)
        {
            if (currenciesDict.TryGetValue(cbrRate.Name, out var existingCurrency))
            {
                existingCurrency.UpdateRate(cbrRate.Rate);
                updatedCount++;
            }
            else
            {
                currenciesToAdd.Add(cbrRate);
            }
        }

        if (currenciesToAdd.Count > 0)
        {
            await repository.AddRange(currenciesToAdd, ct);
            logger.LogInformation("Add {Count} new currencies.", currenciesToAdd.Count);
        }

        await stateSaveChange.SaveChanges(ct);

        logger.LogInformation("Currencies successfully updated. Count: {Count}", updatedCount);
    }
}