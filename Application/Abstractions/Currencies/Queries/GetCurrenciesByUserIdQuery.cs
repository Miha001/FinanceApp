using Finances.Application.Abstractions.Shared;
using Finances.Domain.Db.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Finances.Application.Abstractions.Currencies.Queries;
public sealed record GetCurrenciesByUserIdQuery(Guid UserId) : IRequest<IEnumerable<Currency>>;

public class GetCurrenciesByUserIdHandler(IBaseRepository<User> repository) : IRequestHandler<GetCurrenciesByUserIdQuery, IEnumerable<Currency>>
{
    public async Task<IEnumerable<Currency>> Handle(GetCurrenciesByUserIdQuery request, CancellationToken ct)
    {
        var result = await repository.Query(asNoTracking: false)
            .Include(x => x.UserCurrencies)
            .ThenInclude(x => x.Currencies)
            .Where(x => x.Id == request.UserId)
            .Select(x => x.UserCurrencies.Select(x => x.Currencies)).ToListAsync(ct);

        var currencies =  result.SelectMany(x => x);
        return currencies;
    }
}