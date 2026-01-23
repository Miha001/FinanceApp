using Finances.Application.Abstractions.Repositories;
using Finances.Domain.Db.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Finances.Application.CQRS.Queries;
public sealed record GetAllCurrenciesQuery() : IRequest<List<Currency>>;

public class GetAllCurrenciesHandler(IBaseRepository<Currency> repository) : IRequestHandler<GetAllCurrenciesQuery, List<Currency>>
{
    public Task<List<Currency>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        return repository.Query(false).ToListAsync();
    }
}