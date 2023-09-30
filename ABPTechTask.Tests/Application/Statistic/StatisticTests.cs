using Application.Experiments;
using Application.Statistic;
using AutoFixture;
using Domain;
using EntityFrameworkCore.Testing.Moq.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ABPTechTask.Tests.Application.Statistic;

public class StatisticTests : Test
{
    [Fact]
    public async Task Handle_ValidKey_ReturnsButtonStatistic()
    {
        //Arrange 
        var fixture = new Fixture();

        var json = JsonSerializer.Serialize(fixture.Create<Stat>());

        var expectedResult = new List<Stat> { new Stat { Statistic = json } };


        MockedDbContext.Set<Stat>().AddFromSqlRawResult("ButtonStat", expectedResult);

        var handler = new ButtonColorStatistic.Handler(MockedDbContext);

        var query = new ButtonColorStatistic.Query();

        //Act
        var actualResult = await handler.Handle(query, new CancellationToken());
        JObject jsonObj = JObject.Parse(actualResult);
        
        string formattedResult  = jsonObj.ToString(Formatting.Indented);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(actualResult);
            Assert.Contains(formattedResult, actualResult);
        });
    }
    
    [Fact]
    public async Task Handle_ValidKey_ReturnsPriceStatistic()
    {
        //Arrange 
        var fixture = new Fixture();

        var json = JsonSerializer.Serialize(fixture.Create<Stat>());

        var expectedResult = new List<Stat> { new Stat { Statistic = json } };

        MockedDbContext.Set<Stat>().AddFromSqlRawResult("PriceStat", expectedResult);

        var handler = new PriceStatistic.Handler(MockedDbContext);

        var query = new PriceStatistic.Query();

        //Act
        var actualResult = await handler.Handle(query, new CancellationToken());

        JObject jsonObj = JObject.Parse(actualResult);
        
        string formattedResult  = jsonObj.ToString(Formatting.Indented);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(actualResult);
            Assert.Contains(formattedResult, actualResult);
        });
    }
}