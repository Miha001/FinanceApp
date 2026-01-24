using Finances.Application.Abstractions.Currencies.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Worker.Jobs; 

[DisallowConcurrentExecution]
internal class CurrencyUpdateJob(
    IMediator mediator,
    ILogger<CurrencyUpdateJob> logger
    ) : IJob
{
    public async Task Execute(IJobExecutionContext jobContext)
    {
        try
        {
            logger.LogInformation("Job started");

            await mediator.Send(new UpdateCurrencyRatesCommand(), jobContext.CancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Fail while {nameof(CurrencyUpdateJob)} worked...");
        }
    }
}