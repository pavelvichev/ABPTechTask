using Application.Results;
using AutoFixture;
using EntityFrameworkCore.Testing.Moq.Extensions;

namespace ABPTechTask.Tests.Application.ExperimentResult.Valid
{
   
    public class ExperimentResultTests : Test
    {
        
        [Fact]
        public async Task Handle_ValidKey_ReturnsResult()
        {
            //Arrange
            var fixture = new Fixture();
            var expectedResult = fixture.CreateMany<Domain.ExperimentResult>().ToList();

            MockedDbContext.Set<Domain.ExperimentResult>()
                .AddFromSqlInterpolatedResult($"FindResult {expectedResult.FirstOrDefault()?.DeviceToken}",
                    expectedResult);

            var handler = new FindResult.Handler(MockedDbContext);

            var query = new FindResult.Query { DeviceToken = expectedResult.FirstOrDefault()?.DeviceToken };

            //Act
            var actualResult = await handler.Handle(query, new CancellationToken());

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(actualResult);
                Assert.Equivalent(expectedResult.FirstOrDefault(), actualResult);
            });

        }

        [Fact]
        public async Task Handle_ValidKey_AddResult()
        {
            //Arrange
            var fixture = new Fixture();
            var expectedResult = 1;
            
            var experiment = fixture.Create<Domain.Experiment>();
            MockedDbContext.Set<Domain.Experiment>().AddRange(experiment);
            await MockedDbContext.SaveChangesAsync();

            var result = new Domain.ExperimentResult
            {
                DeviceToken = fixture.Create<string>(),
                ExperimentId = experiment.Id,
                Value = fixture.Create<string>()
            };

           MockedDbContext.AddExecuteSqlRawResult("AddResult",
               expectedResult,
               (providedSql, providedParameters) =>
               {
                   MockedDbContext.Set<Domain.ExperimentResult>().AddRange(result);
                   MockedDbContext.SaveChanges();
               });
           
           var handler = new AddResult.Handler(MockedDbContext);

           var command = new AddResult.Command { ExperimentResult = result };
            
           //Act
           await handler.Handle(command, new CancellationToken());
           
           var addedResult = MockedDbContext.ExperimentResults.FirstOrDefault();

           //Assert
           Assert.Multiple(() => { Assert.Equivalent(addedResult, result); });
        }


    }
}
