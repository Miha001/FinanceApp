using Finances.Domain.Db.Entities;

namespace Finances.Worker.Abstractions;

public interface ICbrClient
{
    Task<List<Currency>> GetCurrencies(CancellationToken ct);
}