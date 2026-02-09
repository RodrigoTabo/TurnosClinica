using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Infrastructure.Configuracion
{
    public class MedicoConfig : IEntityTypeConfiguration<Medico>
    {

        public void Configure(EntityTypeBuilder<Medico> b)
        {

            b.HasKey(x => x.Id);

            b.Property(x => x.Nombre).IsRequired();
            b.Property(x => x.Apellido).IsRequired();
            b.Property(x => x.DNI).IsRequired();
            b.Property(x => x.EspecialidadId);

            b.HasIndex(x => x.DNI).IsUnique();


            b.HasOne(x => x.Especialidad)
                .WithMany(x => x.Medicos)
                .HasForeignKey(x => x.EspecialidadId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
