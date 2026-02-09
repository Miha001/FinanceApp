using Finances.Application.Abstractions.Shared;
using Finances.Application.Extensions;
using Finances.Domain.Db.Entities;
using Finances.Domain.Models;
using Finances.Domain.Models.Dto;
using Finances.Domain.Result;
using MediatR;

namespace Finances.Application.Abstractions.Currencies.Queries;

public sealed record GetAllCurrenciesQuery(
    Pagination pagination) : IRequest<CollectionResult<CourseDto>>;

public class GetAllCurrenciesHandler(IBaseRepository<Currency> repo)
    : IRequestHandler<GetAllCurrenciesQuery, CollectionResult<CourseDto>>
{
    public async Task<CollectionResult<CourseDto>> Handle(GetAllCurrenciesQuery request, CancellationToken ct)
    {
        var totalCount = await repo.GetCount(ct);

        var currencies = await repo.GetAllPaged(
            asNoTracking: true,
            ct,
            request.pagination);

        var dtos = currencies.ToDto();

        return CollectionResult<CourseDto>.Success(dtos, totalCount);
    }
}