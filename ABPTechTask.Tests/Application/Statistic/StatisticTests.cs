using Application.Experiments;
using Application.Statistic;
using AutoFixture;
using Domain;
using EntityFrameworkCore.Testing.Moq.Extensions;

namespace ABPTechTask.Tests.Application.Statistic;

public class StatisticTests : Test
{
    [Fact]
    public async Task Handle_ValidKey_ReturnsButtonStatistic()
    {
        //Arrange 
        var fixture = new Fixture();
        var expectedResult = fixture.CreateMany<Stat>().ToList();


        MockedDbContext.Set<Stat>().AddFromSqlRawResult("ButtonStat", expectedResult);

        var handler = new ButtonColorStatistic.Handler(MockedDbContext);

        var query = new ButtonColorStatistic.Query();

        //Act
        var actualResult = await handler.Handle(query, new CancellationToken());

        //Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(actualResult);
            Assert.Equivalent(expectedResult.FirstOrDefault()?.Statistic, actualResult);
        });
    }
    
    [Fact]
    public async Task Handle_ValidKey_ReturnsPriceStatistic()
    {
        //Arrange 
        var fixture = new Fixture();
        var expectedResult = fixture.CreateMany<Stat>().ToList();


        MockedDbContext.Set<Stat>().AddFromSqlRawResult("PriceStat", expectedResult);

        var handler = new PriceStatistic.Handler(MockedDbContext);

        var query = new PriceStatistic.Query();

        //Act
        var actualResult = await handler.Handle(query, new CancellationToken());

        //Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(actualResult);
            Assert.Equivalent(expectedResult.FirstOrDefault()?.Statistic, actualResult);
        });
    }
}