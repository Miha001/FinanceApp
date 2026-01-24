using Finances.Domain.Db.Entities;

namespace Finances.Application.Abstractions.Shared;

public interface ICbrClient
{
    Task<List<Currency>> GetCurrencies(CancellationToken ct);
}