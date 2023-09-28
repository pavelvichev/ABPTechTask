using Application.Experiments;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABPTechTask.Tests.Application
{
    [Collection("Database collection")]
    public class ApplicationValidTests
    {
        private readonly ABContext _dbContext;

        public ApplicationValidTests()
        {
            DatabaseFixture fixture = new DatabaseFixture();
            _dbContext = fixture.DbContext;
        }


        [Fact]
        public async Task Handle_ValidKey_ReturnsExperiment()
        {
            // Arrange
            var key = "ValidKey";
            var cancellationToken = new CancellationToken();
            var expectedResult = new Experiment { Key = "ValidKey", Options = "Options" };


            var handler = new FindExperiment.Handler(_dbContext);

            // Act
            var query = new FindExperiment.Query { Key = key };
            var result = await handler.Handle(query, cancellationToken);

            // Assert
            Assert.NotNull(result); // Ensure a result is returned
            Assert.Equal(expectedResult, result); // Ensure the result matches the expected result
        }

    }
}
