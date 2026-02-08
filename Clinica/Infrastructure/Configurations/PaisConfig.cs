using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Infrastructure.Configuracion
{
    public class PaisConfig : IEntityTypeConfiguration<Pais>
    {
    

        public void Configure(EntityTypeBuilder<Pais> b)
        {
            b.HasKey(x => x.Id);

            b.HasIndex(x => x.Nombre).IsUnique();

            b.Property(x => x.Nombre).IsRequired();
        }


    }
}
