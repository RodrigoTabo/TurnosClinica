using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Configuracion
{
    public class ConsultorioConfig : IEntityTypeConfiguration<Consultorio>
    {

        public void Configure(EntityTypeBuilder<Consultorio> b)
        {

            b.HasKey(x => x.Id);

            b.Property(x => x.Institucion).IsRequired();
            b.Property(x => x.Calle).IsRequired();
            b.Property(x => x.Altura).IsRequired();

            b.HasIndex(x => new { x.CiudadId, x.Calle, x.Altura }).IsUnique();
            b.HasIndex(x => x.CiudadId);


            b.HasOne(x => x.Ciudad)
                .WithMany(c => c.Consultorios)
                .HasForeignKey(x => x.CiudadId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
