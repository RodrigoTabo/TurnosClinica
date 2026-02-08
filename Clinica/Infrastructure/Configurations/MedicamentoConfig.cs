using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Infrastructure.Configuracion
{
    public class MedicamentoConfig : IEntityTypeConfiguration<Medicamento>
    {


        public void Configure(EntityTypeBuilder<Medicamento> b)
        {


            b.HasKey(x => x.Id);


            b.Property(x => x.Nombre)
                .HasMaxLength(200)
                .IsRequired();


            b.HasMany(x => x.Recetas)
                .WithOne(m => m.Medicamento)
                .HasForeignKey(m => m.MedicamentoId)
                .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
