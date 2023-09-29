using ABPTechTask.Controllers;
using Application.Experiments;
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

        var experimentButtonColor = new Experiment { Id = 1, Key = "button_color" };
        var experimentPrice = new Experiment { Id = 2, Key = "price" };
        var experimentButtonColorResultCount = 10;
        var experimentPriceResultCount = 5;
        
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
        
        var logger = new Mock<ILogger<ExperimentsController>>();

        mediatorMock
            .Setup(m => m.Send(It.IsAny<ExperimentResultsCount.Query>(), default))
            .ReturnsAsync((ExperimentResultsCount.Query query, System.Threading.CancellationToken cancellationToken) =>
            {
                if (query.ExperimentId == experimentButtonColor.Id)
                    return experimentButtonColorResultCount;
                else if (query.ExperimentId == experimentPrice.Id)
                    return experimentPriceResultCount;

                return 0;
            });

        var controller = new ExperimentsController(mediatorMock.Object,logger.Object);

        // Act
        var result = await controller.GetStatistic();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var statistic = Assert.IsType<Dictionary<string, int>>(okResult.Value);

        Assert.Equal(experimentButtonColorResultCount + experimentPriceResultCount, statistic["Всі пристрої"]);
        Assert.Equal(experimentButtonColorResultCount, statistic["Експеримент з кольором кнопки"]);
        Assert.Equal(experimentPriceResultCount, statistic["Експеримент з ціною"]);
    }

}