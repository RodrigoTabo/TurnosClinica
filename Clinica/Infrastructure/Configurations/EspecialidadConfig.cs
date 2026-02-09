using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Infrastructure.Configuracion
{
    public class EspecialidadConfig : IEntityTypeConfiguration<Especialidad> 
    {

        public void Configure(EntityTypeBuilder<Especialidad> b)
        {

            b.HasKey(x => x.Id);

            b.Property(x => x.Nombre).IsRequired();

            b.HasIndex(x => x.Nombre).IsUnique();

        }

    }
}
