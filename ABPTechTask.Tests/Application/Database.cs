using EntityFrameworkCore.Testing.Moq;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace ABPTechTask.Tests.Application
{
    public class Test
    {
        protected readonly ABContext MockedDbContext;
        protected Test()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ABContext>().UseInMemoryDatabase("Hello").Options;
            MockedDbContext = Create.MockedDbContextFor<ABContext>(dbContextOptions);
        }
    }
}
