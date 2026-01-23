using Finances.Application.Abstractions.Currencies;
using Finances.Application.Abstractions.Currencies.Queries;
using Finances.Application.Abstractions.Users.Commands;
using Finances.Domain.Db.Entities;
using Finances.Domain.Models.VM;
using Finances.Domain.Result;
using MediatR;

namespace Finances.DAL.Implementations.Carrencies;
public class CurrenciesService(IMediator mediator) : ICurrenciesService
{
    public async Task<DataResult<bool>> AddToFavorites(Guid userId, Guid currencyId)
    {
        await mediator.Send(new CreateUserCurrencyCommand(userId, currencyId));

        return DataResult<bool>.Success(true);
    }

    public async Task<CollectionResult<CourseVM>> GetAll()
    {
        var result = await mediator.Send(new GetAllCurrenciesQuery());

        return CollectionResult<CourseVM>.Success(EntityToVM(result));
    }

    public async Task<CollectionResult<CourseVM>> GetCoursesByUserId(Guid userId)
    {
        var result = await mediator.Send(new GetCurrenciesByUserIdQuery(userId));

        return CollectionResult<CourseVM>.Success(EntityToVM(result));
    }

    private List<CourseVM> EntityToVM(IEnumerable<Currency> currencies)
    {
        return currencies.Select(x => new CourseVM { Id = x.Id, Name = x.Name, Rate = x.Rate }).ToList();
    }
}