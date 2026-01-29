using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Configuracion
{
    public class HistoriaClinicaConfig : IEntityTypeConfiguration<HistoriaClinica>
    {

        public void Configure(EntityTypeBuilder<HistoriaClinica> b)
        {

            //1) PK

            b.HasKey(x => x.Id);

            //2) Propiedades (Reglas duras del modelo)
            b.Property(x => x.Observaciones)
                .HasMaxLength(2000); //Ajusta un maximo limite.


            b.Property(x => x.PacienteId).IsRequired();


            b.Property(x => x.Alergias)
                .HasMaxLength(2000);


            // 4) Relación con Paciente
            // Queremos: Paciente (1) <-> (1) HistoriaClinica
            // - HistoriaClinica tiene la FK PacienteId
            // - Paciente tiene la navegación HistoriaClinica (propiedad)
            b.HasOne(x => x.Paciente)
                .WithOne(p => p.HistoriaClinica)
                .HasForeignKey<HistoriaClinica>(x => x.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);


            // 5) “Blindaje” para que sea 1-1 real en la DB
            // (Sin esto, podrías terminar teniendo varias HistoriasClinicas para el mismo Paciente)
            b.HasIndex(x => x.PacienteId)
                .IsUnique();


            // 6) Relación con Recetas
            // Queremos: HistoriaClinica (1) -> (N) Receta
            // (Esto requiere que Receta tenga HistoriaClinicaId + HistoriaClinica)
            b.HasMany(x => x.Recetas)
                .WithOne(x => x.HistoriaClinica)
                .HasForeignKey(r => r.HistoriaClinicaId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
