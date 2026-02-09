using Finances.Application.Abstractions.Currencies.Queries;
using Finances.Application.Abstractions.Shared;
using Finances.Domain.Db.Entities;
using Finances.Domain.Models;
using Moq;
using Tests.Helpers;

namespace Tests.UnitTest;

public class GetAllCurrenciesHandlerTests
{
    private readonly Mock<IBaseRepository<Currency>> _repoMock;
    private readonly GetAllCurrenciesHandler _handler;

    public GetAllCurrenciesHandlerTests()
    {
        _repoMock = new Mock<IBaseRepository<Currency>>();
        _handler = new GetAllCurrenciesHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_SHouldReturnMappedCurrencies()
    {
        // Arrange
        var currencyId1 = Guid.NewGuid();
        var currencyId2 = Guid.NewGuid();

        var pagination = new Pagination();
        var currencies = new List<Currency>
        {
            new Currency("USD", 1.0m).SetId(currencyId1),
            new Currency("EUR", 0.85m).SetId(currencyId2)
        };

        _repoMock.Setup(r => r.GetAllPaged(It.IsAny<bool>(), It.IsAny<CancellationToken>(), pagination))
            .ReturnsAsync(currencies);

        var query = new GetAllCurrenciesQuery(pagination);

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.TotalCount);

        var usdDto = result.Data.FirstOrDefault(c => c.Name == "USD");
        Assert.NotNull(usdDto);
        Assert.Equal(1.0m, usdDto.Rate);
    }
}
