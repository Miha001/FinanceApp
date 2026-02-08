using Finances.Application.Abstractions.Shared;
using Finances.Application.Extensions;
using Finances.Domain.Models.Dto;
using Finances.Domain.Result;
using MediatR;

namespace Finances.Application.Abstractions.Currencies.Queries;
public sealed record GetCurrenciesByUserIdQuery() : IRequest<CollectionResult<CourseDto>>;

public class GetCurrenciesByUserIdHandler(
    ICurrentUserProvider currentUserProvider,
    ICurrenciesRepository repository
    ) : IRequestHandler<GetCurrenciesByUserIdQuery, CollectionResult<CourseDto>>
{
    public async Task<CollectionResult<CourseDto>> Handle(GetCurrenciesByUserIdQuery request, CancellationToken ct)
    {
        var userId = currentUserProvider.UserId;
        var currencies = await repository.GetByUserId(userId, ct);

        var dtos = currencies.ToDto();
        return CollectionResult<CourseDto>.Success(dtos);
    }
}