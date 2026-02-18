using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurnosClinica.Migrations
{
    /// <inheritdoc />
    public partial class AddRefactorCiudad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ciudades_ProvinciaId_Nombre",
                table: "Ciudades");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Ciudades",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<DateTime>(
                name: "EliminadoEn",
                table: "Ciudades",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ciudades_ProvinciaId_Nombre",
                table: "Ciudades",
                columns: new[] { "ProvinciaId", "Nombre" },
                unique: true,
                filter: "[EliminadoEn] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ciudades_ProvinciaId_Nombre",
                table: "Ciudades");

            migrationBuilder.DropColumn(
                name: "EliminadoEn",
                table: "Ciudades");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Ciudades",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120);

            migrationBuilder.CreateIndex(
                name: "IX_Ciudades_ProvinciaId_Nombre",
                table: "Ciudades",
                columns: new[] { "ProvinciaId", "Nombre" },
                unique: true);
        }
    }
}
