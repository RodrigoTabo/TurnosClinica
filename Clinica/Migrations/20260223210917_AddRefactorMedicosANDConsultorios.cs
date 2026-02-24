using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurnosClinica.Migrations
{
    /// <inheritdoc />
    public partial class AddRefactorMedicosANDConsultorios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConsultorioId",
                table: "Medicos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_ConsultorioId",
                table: "Medicos",
                column: "ConsultorioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicos_Consultorios_ConsultorioId",
                table: "Medicos",
                column: "ConsultorioId",
                principalTable: "Consultorios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicos_Consultorios_ConsultorioId",
                table: "Medicos");

            migrationBuilder.DropIndex(
                name: "IX_Medicos_ConsultorioId",
                table: "Medicos");

            migrationBuilder.DropColumn(
                name: "ConsultorioId",
                table: "Medicos");
        }
    }
}
