using ABPTechTask.Controllers;
using Application.Experiments;
using Application.Results;
using Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ABPTechTask.Tests.Controller.Invalid;

public class ControllerInvalidButtonTest
{
    [Fact]
    public async Task ButtonColorExperiment_ThrowException()
    {
        // Arrange
        var deviceToken = "your-device-token";

        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<FindExperiment.Query>(), default))
            .ThrowsAsync(new Exception("Simulated exception"));
        
        
        
        var controller = new ExperimentsController(mediatorMock.Object);

     await  controller.Invoking(x => x.ButtonColorExperimnt(deviceToken)).Should().ThrowAsync<Exception>();
    }
    
}