using EntityFrameworkCore.Testing.Moq;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace ABPTechTask.Tests.Application
{
    public class Test
    {
        protected readonly AbContext MockedDbContext;
        protected Test()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AbContext>().UseInMemoryDatabase("Hello").Options;
            MockedDbContext = Create.MockedDbContextFor<AbContext>(dbContextOptions);
        }
    }
}
