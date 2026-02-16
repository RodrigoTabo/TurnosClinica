using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Infrastructure.Configuracion
{
    public class PacienteConfig : IEntityTypeConfiguration<Paciente>
    {

        public void Configure(EntityTypeBuilder<Paciente> b)
        {

            b.HasKey(x => x.Id);

            b.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            b.Property(x => x.Apellido).HasMaxLength(50).IsRequired();
            b.Property(x => x.EmailPrincipal).HasMaxLength(254);
            b.Property(x => x.EmailPendiente).HasMaxLength(254);

            b.HasIndex(x => x.DNI).IsUnique();


        }

    }
}
