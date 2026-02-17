using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurnosClinica.Migrations
{
    /// <inheritdoc />
    public partial class AddRefactorPais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Paises_Nombre",
                table: "Paises");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Paises",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Paises_Nombre",
                table: "Paises",
                column: "Nombre",
                unique: true,
                filter: "[EliminadoEn] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Paises_Nombre",
                table: "Paises");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Paises",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120);

            migrationBuilder.CreateIndex(
                name: "IX_Paises_Nombre",
                table: "Paises",
                column: "Nombre",
                unique: true);
        }
    }
}
