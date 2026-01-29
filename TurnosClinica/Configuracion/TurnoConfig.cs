using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Configuracion
{
    public class TurnoConfig : IEntityTypeConfiguration<Turno>
    {

        public void Configure(EntityTypeBuilder<Turno> b)
        {

            b.HasKey(x => x.Id);

            b.Property(x => x.FechaTurno).IsRequired().HasColumnType("datetime2(0)");


            b.HasIndex(x => new { x.MedicoId, x.FechaTurno }).IsUnique();
            b.HasIndex(x => new { x.PacienteId, x.FechaTurno }).IsUnique();
            b.HasIndex(x => new { x.ConsultorioId, x.FechaTurno }).IsUnique();
            b.HasIndex(x => x.FechaTurno);



            b.HasOne(x => x.Paciente)
                .WithMany(p => p.Turnos)
                .HasForeignKey(x => x.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Medico)
                .WithMany(p => p.Turnos)
                .HasForeignKey(x => x.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Consultorio)
                .WithMany(p => p.Turnos)
                .HasForeignKey(x => x.ConsultorioId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Estado)
                .WithMany(p => p.Turnos)
                .HasForeignKey(x => x.EstadoId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(t => t.Pago)
                .WithMany(p => p.Turnos)
                .HasForeignKey(t => t.PagoId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
