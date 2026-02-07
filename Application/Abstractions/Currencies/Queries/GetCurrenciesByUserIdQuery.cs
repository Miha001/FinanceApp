using Finances.Application.Extensions;
using Finances.Domain.Models.Dto;
using Finances.Domain.Result;
using MediatR;

namespace Finances.Application.Abstractions.Currencies.Queries;
public sealed record GetCurrenciesByUserIdQuery(Guid UserId) : IRequest<CollectionResult<CourseDto>>;

public class GetCurrenciesByUserIdHandler(
    ICurrenciesRepository repository
    ) : IRequestHandler<GetCurrenciesByUserIdQuery, CollectionResult<CourseDto>>
{
    public async Task<CollectionResult<CourseDto>> Handle(GetCurrenciesByUserIdQuery request, CancellationToken ct)
    {
        var currencies = await repository.GetByUserId(request.UserId, ct);

        var dtos = currencies.ToDto();
        return CollectionResult<CourseDto>.Success(dtos);
    }
}