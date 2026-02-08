using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Infrastructure.Configuracion
{
    public class CiudadConfig : IEntityTypeConfiguration<Ciudad>
    {
        public void Configure(EntityTypeBuilder<Ciudad> b)
        {

            b.HasKey(x => x.Id);

            b.Property(x => x.Nombre).IsRequired();

            b.HasIndex(x => new { x.ProvinciaId, x.Nombre }).IsUnique();

            b.HasIndex(x => x.ProvinciaId);

            b.HasOne(x => x.Provincia)
                .WithMany(x => x.Ciudades)
                .HasForeignKey(x => x.ProvinciaId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}