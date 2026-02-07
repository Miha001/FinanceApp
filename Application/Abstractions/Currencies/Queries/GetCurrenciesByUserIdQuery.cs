using Finances.Domain.Db.Entities;
using MediatR;

namespace Finances.Application.Abstractions.Currencies.Queries;
public sealed record GetCurrenciesByUserIdQuery(Guid UserId) : IRequest<IReadOnlyCollection<Currency>>;

public class GetCurrenciesByUserIdHandler(
    ICurrenciesRepository repository
    ) : IRequestHandler<GetCurrenciesByUserIdQuery, IReadOnlyCollection<Currency>>
{
    public async Task<IReadOnlyCollection<Currency>> Handle(GetCurrenciesByUserIdQuery request, CancellationToken ct)
    {
        return await repository.GetByUserId(request.UserId, ct);
    }
}