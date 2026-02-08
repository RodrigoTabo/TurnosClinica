using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Infrastructure.Configuracion
{
    public class RecetaConfig : IEntityTypeConfiguration<Receta>
    {

        public void Configure(EntityTypeBuilder<Receta> b)
        {

            b.HasKey(x => x.Id);

            b.Property(x => x.Observaciones)
                .HasMaxLength(2000);

            b.Property(x => x.Fecha)
                .IsRequired();

            b.HasOne(x => x.HistoriaClinica)
                .WithMany(h => h.Recetas)
                .HasForeignKey(x => x.HistoriaClinicaId)
                .OnDelete(DeleteBehavior.Restrict);

            // índice normal (no unique) para performance
            b.HasIndex(x => x.HistoriaClinicaId);

        }


    }
}
