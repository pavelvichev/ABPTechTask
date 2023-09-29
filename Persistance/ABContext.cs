
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistance
{
    public class AbContext : DbContext
    {
        public AbContext()
        {
        }
        public AbContext(DbContextOptions<AbContext> options) : base (options) { }
       
        public virtual DbSet<ExperimentResult> ExperimentResults { get; set; }
        public virtual DbSet<Experiment> Experiments { get; set; }
        public virtual DbSet<Stat> Statistics { get; set; }
        
        
    }
}
