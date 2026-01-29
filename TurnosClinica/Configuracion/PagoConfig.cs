using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurnosClinica.Models;

namespace TurnosClinica.Configuracion
{
    public class PagoConfig : IEntityTypeConfiguration<Pago>
    {

        public void Configure(EntityTypeBuilder<Pago> b)
        {

            b.HasKey(x => x.Id);


            b.HasOne(x => x.PagoMetodo)
                .WithMany(pm => pm.Pagos)
                .HasForeignKey(x => x.PagoMetodoId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
