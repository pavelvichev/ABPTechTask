using Application.Experiments;
using AutoFixture;
using Domain;
using EntityFrameworkCore.Testing.Moq.Extensions;

namespace ABPTechTask.Tests.Application.Valid;

public class ExperimntTests : Test
{
    [Fact]
    public async Task Handle_ValidKey_ReturnsExperiment()
    {
        var fixture = new Fixture();
        var expectedResult = fixture.CreateMany<Experiment>().ToList();
           

        _mockedDbContext.Set<Experiment>()
            .AddFromSqlInterpolatedResult($"FindExperiment {expectedResult.FirstOrDefault().Key}", expectedResult);

        var handler = new FindExperiment.Handler(_mockedDbContext);

        var query = new FindExperiment.Query { Key = expectedResult.FirstOrDefault().Key };

        var actualResult = await handler.Handle(query, new CancellationToken());

        Assert.Multiple(() =>
        {
            Assert.NotNull(actualResult);
            Assert.Equivalent(expectedResult.FirstOrDefault(), actualResult);
        });

    }

}