using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurnosClinica.Migrations
{
    /// <inheritdoc />
    public partial class AddRefactorPacientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EliminadoEn",
                table: "Pacientes",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EliminadoEn",
                table: "Pacientes");
        }
    }
}
