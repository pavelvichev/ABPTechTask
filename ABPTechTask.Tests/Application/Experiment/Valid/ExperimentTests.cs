using Application.Experiments;
using AutoFixture;
using Domain;
using EntityFrameworkCore.Testing.Moq.Extensions;

namespace ABPTechTask.Tests.Application.Experiment.Valid;

public class ExperimentTests : Test
{
    [Fact]
    public async Task Handle_ValidKey_ReturnsExperiment()
    {
        //Arrange 
        var fixture = new Fixture();
        var expectedResult = fixture.CreateMany<Domain.Experiment>().ToList();

        
            MockedDbContext.Set<Domain.Experiment>()
                .AddFromSqlInterpolatedResult($"FindExperiment {expectedResult.FirstOrDefault()?.Key}", expectedResult);

            var handler = new FindExperiment.Handler(MockedDbContext);

            var query = new FindExperiment.Query { Key = expectedResult.FirstOrDefault()?.Key };

            //Act
            var actualResult = await handler.Handle(query, new CancellationToken());

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(actualResult);
                Assert.Equivalent(expectedResult.FirstOrDefault(), actualResult);
            });

        }
    }
    