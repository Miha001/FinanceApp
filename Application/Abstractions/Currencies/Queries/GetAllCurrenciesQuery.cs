using Finances.Application.Abstractions.Shared;
using Finances.Domain.Db.Entities;
using MediatR;

namespace Finances.Application.Abstractions.Currencies.Queries;
public sealed record GetAllCurrenciesQuery() : IRequest<IReadOnlyCollection<Currency>>;

public class GetAllCurrenciesHandler(IBaseRepository<Currency> repository) : IRequestHandler<GetAllCurrenciesQuery, IReadOnlyCollection<Currency>>
{
    public async Task<IReadOnlyCollection<Currency>> Handle(GetAllCurrenciesQuery request, CancellationToken ct)
    {
        return await repository.GetAll(false, ct);
    }
}