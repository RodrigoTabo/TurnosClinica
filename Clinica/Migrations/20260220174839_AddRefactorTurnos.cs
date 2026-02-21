using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurnosClinica.Migrations
{
    /// <inheritdoc />
    public partial class AddRefactorTurnos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EliminadoEn",
                table: "Turnos",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EliminadoEn",
                table: "Turnos");
        }
    }
}
