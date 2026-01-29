using Microsoft.EntityFrameworkCore;

namespace TurnosClinica.Data
{
    public class TurnosDbContext : DbContext
    {

        public TurnosDbContext(DbContextOptions<TurnosDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TurnosDbContext).Assembly);
        }




    }
}
