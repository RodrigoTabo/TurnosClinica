using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurnosClinica.Migrations
{
    /// <inheritdoc />
    public partial class PacientesYReglasTurnos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DNI",
                table: "Pacientes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "EmailPendiente",
                table: "Pacientes",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmailPrincipal",
                table: "Pacientes",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailPendiente",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "EmailPrincipal",
                table: "Pacientes");

            migrationBuilder.AlterColumn<int>(
                name: "DNI",
                table: "Pacientes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
