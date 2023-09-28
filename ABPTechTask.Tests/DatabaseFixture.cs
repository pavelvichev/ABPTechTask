using Microsoft.EntityFrameworkCore;
using Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABPTechTask.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public ABContext DbContext { get; private set; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<ABContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") // Используйте In-Memory базу данных для тестов
                .Options;

            DbContext = new ABContext(options);

            // Здесь можно добавить код для создания начальных данных в тестовой базе данных (если требуется).
        }

        public void Dispose()
        {
            // Закрыть и освободить ресурсы базы данных (если требуется).
            DbContext.Dispose();
        }
    }
}
