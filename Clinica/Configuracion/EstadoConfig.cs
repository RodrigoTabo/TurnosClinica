using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Configuracion
{
    public class EstadoConfig : IEntityTypeConfiguration<Estado>
    {

        public void Configure(EntityTypeBuilder<Estado> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Nombre).IsRequired();

            b.HasIndex(x => x.Nombre).IsUnique();
        }

    }
}
