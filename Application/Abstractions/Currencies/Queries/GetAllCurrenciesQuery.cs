using Finances.Application.Abstractions.Shared;
using Finances.Application.Extensions;
using Finances.Domain.Db.Entities;
using Finances.Domain.Models.Dto;
using Finances.Domain.Result;
using MediatR;

namespace Finances.Application.Abstractions.Currencies.Queries;

public sealed record GetAllCurrenciesQuery() : IRequest<CollectionResult<CourseDto>>;

public class GetAllCurrenciesHandler(IBaseRepository<Currency> repository)
    : IRequestHandler<GetAllCurrenciesQuery, CollectionResult<CourseDto>>
{
    public async Task<CollectionResult<CourseDto>> Handle(GetAllCurrenciesQuery request, CancellationToken ct)
    {
        var currencies = await repository.GetAll(false, ct);

        var dtos = currencies.ToDto();

        return CollectionResult<CourseDto>.Success(dtos);
    }
}