using Application.CQRS.Commands;
using Application.CQRS.Queries;
using DAL.Implementations.Services;
using Domain.Db.Entities;
using MediatR;
using Moq;

namespace Tests.Configurations.Fixture;
public class CurrenciesServiceFixture : IDisposable
{
    public Mock<IMediator> MediatorMock { get; }
    public CurrenciesService CurrenciesService { get; }

    public CurrenciesServiceFixture()
    {
        MediatorMock = new Mock<IMediator>();
        CurrenciesService = new CurrenciesService(MediatorMock.Object);
    }

    public void SetupGetAllCurrencies(IEnumerable<Currency> currencies)
    {
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllCurrenciesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencies.ToList());
    }

    public void SetupGetCurrenciesByUserId(Guid userId, IEnumerable<Currency> currencies)
    {
        MediatorMock
            .Setup(m => m.Send(
                It.Is<GetCurrenciesByUserIdQuery>(q => q.UserId == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencies);
    }

    public void SetupCreateUserCurrency(Guid userId, Guid currencyId)
    {
        MediatorMock
            .Setup(m => m.Send(
                It.Is<CreateUserCurrencyCommand>(c => c.UserId == userId && c.CurrencyId == currencyId),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    public void Dispose() { }
}