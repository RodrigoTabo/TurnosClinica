using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Configuracion
{
    public class PagoMetodoConfig : IEntityTypeConfiguration<PagoMetodo>
    {

        public void Configure(EntityTypeBuilder<PagoMetodo> b)
        {

            b.HasKey(x => x.Id);

            b.Property(x => x.Nombre).IsRequired();

            b.HasIndex(x => x.Nombre).IsUnique();

        }

    }
}
