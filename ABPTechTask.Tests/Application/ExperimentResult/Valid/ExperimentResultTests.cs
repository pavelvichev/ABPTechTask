using Application.Experiments;
using Domain;

using EntityFrameworkCore.Testing.Moq;
using Moq;
using Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Results;
using AutoFixture;
using EntityFrameworkCore.Testing.Moq.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ABPTechTask.Tests.Application
{
    [Collection("Database collection")]
    public class ExperimentResultTests : Test
    {
        
        [Fact]
        public async Task Handle_ValidKey_ReturnsResult()
        {
            var fixture = new Fixture();
            var expectedResult = fixture.CreateMany<ExperimentResult>().ToList();

            _mockedDbContext.Set<ExperimentResult>()
                .AddFromSqlInterpolatedResult($"FindResult {expectedResult.FirstOrDefault().DeviceToken}",
                    expectedResult);

            var handler = new FindResult.Handler(_mockedDbContext);

            var query = new FindResult.Query { DeviceToken = expectedResult.FirstOrDefault().DeviceToken };

            var actualResult = await handler.Handle(query, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.NotNull(actualResult);
                Assert.Equivalent(expectedResult.FirstOrDefault(), actualResult);
            });

        }

        [Fact]
        public async Task Handle_ValidKey_AddResult()
        {
            var fixture = new Fixture();
            var expectedResult = 1;
            
            var experiment = fixture.Create<Experiment>();
            _mockedDbContext.Set<Experiment>().AddRange(experiment);
            _mockedDbContext.SaveChanges();

            var result = new ExperimentResult
            {
                DeviceToken = fixture.Create<string>(),
                ExperimentId = experiment.Id,
                Value = fixture.Create<string>()
            };

           _mockedDbContext.AddExecuteSqlRawResult("AddResult",
               expectedResult,
               (providedSql, providedParameters) =>
               {
                   _mockedDbContext.Set<ExperimentResult>().AddRange(result);
                   _mockedDbContext.SaveChanges();
               });
           
           var handler = new AddResult.Handler(_mockedDbContext);

           var command = new AddResult.Command { ExperimentResult = result };
            
           await handler.Handle(command, new CancellationToken());
           
           var addedResult = _mockedDbContext.ExperimentResults.FirstOrDefault();

           Assert.Multiple(() => { Assert.Equivalent(addedResult, result); });
        }


    }
}
