using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Configuracion
{
    public class RecetaMedicamentoConfig : IEntityTypeConfiguration<RecetaMedicamento>
    {

        public void Configure(EntityTypeBuilder<RecetaMedicamento> b)
        {

            b.HasKey(x => x.Id);

            b.Property(x => x.Dosis).IsRequired();
            b.Property(x => x.Dias).IsRequired();

            b.HasIndex(x => new { x.RecetaId, x.MedicamentoId }).IsUnique();


            b.HasOne(x => x.Receta)
                .WithMany(r => r.Items)
                .HasForeignKey(x => x.RecetaId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Medicamento)
                .WithMany(r => r.Recetas)
                .HasForeignKey(x => x.MedicamentoId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
