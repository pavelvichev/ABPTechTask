using Microsoft.EntityFrameworkCore;
using Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkCore.Testing.Moq;

namespace ABPTechTask.Tests
{
    public class Test
    {
        protected readonly ABContext _mockedDbContext;
        protected Test()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ABContext>().UseInMemoryDatabase("Hello").Options;
            _mockedDbContext = Create.MockedDbContextFor<ABContext>(dbContextOptions);
        }
    }
}
