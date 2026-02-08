using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Infrastructure.Configuracion
{
    public class ProvinciaConfig : IEntityTypeConfiguration<Provincia>
    {

        public void Configure(EntityTypeBuilder<Provincia> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Nombre).IsRequired();

            b.HasIndex(x => new { x.PaisId, x.Nombre }).IsUnique();

            b.HasIndex(x => x.PaisId);


            b.HasOne(x => x.Pais)
                .WithMany(x => x.Provincias)
                .HasForeignKey(x => x.PaisId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
