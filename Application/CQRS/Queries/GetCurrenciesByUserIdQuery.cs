using Application.Abstractions.Repositories;
using Domain.Db.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries;
public sealed record GetCurrenciesByUserIdQuery(Guid UserId) : IRequest<IEnumerable<Currency>>;

public class GetCurrenciesByUserIdHandler(IBaseRepository<User> repository) : IRequestHandler<GetCurrenciesByUserIdQuery, IEnumerable<Currency>>
{
    public async Task<IEnumerable<Currency>> Handle(GetCurrenciesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.Query(asNoTracking: false)
            .Include(x => x.UserCurrencies)
            .ThenInclude(x => x.Currencies)
            .Where(x => x.Id == request.UserId)
            .Select(x => x.UserCurrencies.Select(x => x.Currencies)).ToListAsync();

        var currencies =  result.SelectMany(x => x);
        return currencies;
    }
}