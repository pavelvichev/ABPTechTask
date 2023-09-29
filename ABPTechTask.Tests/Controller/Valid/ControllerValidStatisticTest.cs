using ABPTechTask.Controllers;
using Application.Experiments;
using Application.Statistic;
using Application.Statistic.Results;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ABPTechTask.Tests.Controller.Valid;

public class ControllerValidStatisticTest
{
    [Fact]
    public async Task GetStatistic_ReturnsCorrectStatistics()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var logger = new Mock<ILogger<ExperimentsController>>();

        var experimentButtonColor = new Experiment { Id = 1, Key = "button_color" };
        var experimentPrice = new Experiment { Id = 2, Key = "price" };
        var experimentButtonColorResultCount = 10;
        var experimentPriceResultCount = 5;

        var buttonStat = "buttonStat"; // Replace with your test data
        var priceStat = "priceStat"; // Replace with your test data

        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindExperiment.Query>(), default))
            .ReturnsAsync((FindExperiment.Query query, System.Threading.CancellationToken cancellationToken) =>
            {
                if (query.Key == "button_color")
                    return experimentButtonColor;
                else if (query.Key == "price")
                    return experimentPrice;

                return null;
            });

        mediatorMock
            .Setup(m => m.Send(It.IsAny<ExperimentResultsCount.Query>(), default))
            .ReturnsAsync(experimentPriceResultCount);

        mediatorMock
            .Setup(m => m.Send(It.IsAny<ButtonColorStatistic.Query>(), default))
            .ReturnsAsync(buttonStat);

        mediatorMock
            .Setup(m => m.Send(It.IsAny<PriceStatistic.Query>(), default))
            .ReturnsAsync(priceStat);

        var controller = new ExperimentsController(mediatorMock.Object, logger.Object);

        // Act
        var result = await controller.GetStatistic();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var statistic = Assert.IsType<string>(okResult.Value);

        Assert.Contains($"{buttonStat}", statistic);
        Assert.Contains($"{priceStat}", statistic);
        Assert.Contains($"Total Devices : {experimentPriceResultCount}", statistic);
    }

}