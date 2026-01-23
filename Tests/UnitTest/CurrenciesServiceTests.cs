using Finances.Application.Abstractions.Currencies.Queries;
using Finances.Application.Abstractions.Users.Commands;
using Finances.Domain.Db.Entities;
using Moq;
using Tests.Configurations.Fixture;

namespace Tests.UnitTest;
public class CurrenciesServiceTests : IClassFixture<CurrenciesServiceFixture>
{
    private readonly CurrenciesServiceFixture _fixture;

    public CurrenciesServiceTests(CurrenciesServiceFixture fixture)
    {
        _fixture = fixture;
        _fixture.MediatorMock.Invocations.Clear();
    }

    [Fact]
    public async Task AddToFavorites_ShouldSendCreateCommandAndReturnSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var currencyId = Guid.NewGuid();

        _fixture.SetupCreateUserCurrency(userId, currencyId);

        // Act
        var result = await _fixture.CurrenciesService.AddToFavorites(userId, currencyId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Data);

        _fixture.MediatorMock.Verify(m => m.Send(
            It.Is<CreateUserCurrencyCommand>(c => c.UserId == userId && c.CurrencyId == currencyId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllCurrenciesMappedToCourseVM()
    {
        // Arrange
        var currency1Id = Guid.NewGuid();
        var currency2Id = Guid.NewGuid();
        var currencies = new List<Currency>
        {
            new Currency { Id = currency1Id, Name = "USD", Rate = 1.0m },
            new Currency { Id = currency2Id, Name = "EUR", Rate = 0.85m }
        };

        _fixture.SetupGetAllCurrencies(currencies);

        // Act
        var result = await _fixture.CurrenciesService.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count());

        var usdVm = result.Data.FirstOrDefault(c => c.Id == currency1Id);
        Assert.NotNull(usdVm);
        Assert.Equal("USD", usdVm.Name);
        Assert.Equal(1.0m, usdVm.Rate);

        _fixture.MediatorMock.Verify(m => m.Send(
            It.IsAny<GetAllCurrenciesQuery>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyCollectionWhenNoCurrencies()
    {
        // Arrange
        var currencies = new List<Currency>();
        _fixture.SetupGetAllCurrencies(currencies);

        // Act
        var result = await _fixture.CurrenciesService.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);
    }

    [Fact]
    public async Task GetCoursesByUserId_ShouldReturnUserCurrenciesMappedToCourseVM()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var currencyId = Guid.NewGuid();
        var userCurrencies = new List<Currency>
        {
            new Currency { Id = currencyId, Name = "GBP", Rate = 0.75m }
        };

        _fixture.SetupGetCurrenciesByUserId(userId, userCurrencies);

        // Act
        var result = await _fixture.CurrenciesService.GetCoursesByUserId(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Data);

        var gbpVm = result.Data.FirstOrDefault(c => c.Id == currencyId);
        Assert.NotNull(gbpVm);
        Assert.Equal("GBP", gbpVm.Name);
        Assert.Equal(0.75m, gbpVm.Rate);

        _fixture.MediatorMock.Verify(m => m.Send(
            It.Is<GetCurrenciesByUserIdQuery>(q => q.UserId == userId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCoursesByUserId_ShouldReturnEmptyCollectionWhenUserHasNoFavorites()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userCurrencies = new List<Currency>();
        _fixture.SetupGetCurrenciesByUserId(userId, userCurrencies);

        // Act
        var result = await _fixture.CurrenciesService.GetCoursesByUserId(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);

        _fixture.MediatorMock.Verify(m => m.Send(
            It.Is<GetCurrenciesByUserIdQuery>(q => q.UserId == userId),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}