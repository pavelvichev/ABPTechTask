using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistance
{
    public class ABContext : DbContext
    {
        public ABContext()
        {
        }
        public ABContext(DbContextOptions<ABContext> options) : base (options) { }
       
        public DbSet<ExperimentResult> ExperimentResults { get; set; }
        public DbSet<Experiment> Experiments { get; set; }

    }
}
