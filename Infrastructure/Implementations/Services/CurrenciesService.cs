using Application.Abstractions.Services;
using Application.CQRS.Commands;
using Application.CQRS.Queries;
using Application.Resources;
using Domain.Db.Entities;
using Domain.Models.VM;
using Domain.Result;
using MediatR;

namespace DAL.Implementations.Services;
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