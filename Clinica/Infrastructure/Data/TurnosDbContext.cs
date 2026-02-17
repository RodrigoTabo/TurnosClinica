using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TurnosClinica.Domain.Entities.Interfaces;
using TurnosClinica.Models;

namespace TurnosClinica.Infrastructure.Data
{
    public class TurnosDbContext : IdentityDbContext
    {
        public TurnosDbContext(DbContextOptions<TurnosDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TurnosDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var prop = Expression.Property(parameter, nameof(ISoftDelete.EliminadoEn));
                    var nullConst = Expression.Constant(null, typeof(DateTime?));
                    var body = Expression.Equal(prop, nullConst);

                    var lambda = Expression.Lambda(body, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

        }

        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Consultorio> Consultorios { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<HistoriaClinica> HistoriaClinicas { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<PagoMetodo> PagoMetodos { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<RecetaMedicamento> RecetaMedicamentos { get; set; }
        public DbSet<Turno> Turnos { get; set; }
    }



}

