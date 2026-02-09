using Finances.Application.Abstractions.Shared;
using Finances.Application.Extensions;
using Finances.Domain.Db.Entities;
using Finances.Domain.Models;
using Finances.Domain.Models.Dto;
using Finances.Domain.Result;
using MediatR;

namespace Finances.Application.Abstractions.Currencies.Queries;
public sealed record GetCurrenciesByUserIdQuery(Pagination pagination) : IRequest<CollectionResult<CourseDto>>;

public class GetCurrenciesByUserIdHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<UserCurrency> favCurrencyRepo,
    ICurrenciesRepository repo
    ) : IRequestHandler<GetCurrenciesByUserIdQuery, CollectionResult<CourseDto>>
{
    public async Task<CollectionResult<CourseDto>> Handle(GetCurrenciesByUserIdQuery request, CancellationToken ct)
    {
        var userId = currentUserProvider.UserId;
        var totalCount = await favCurrencyRepo.GetCount(ct, x => x.UserId == userId);
        var currencies = await repo.GetFavoriteByUserId(userId, ct, request.pagination);

        var dtos = currencies.ToDto();
        return CollectionResult<CourseDto>.Success(dtos, totalCount);
    }
}