using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DGTickets.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsImageSquaredColumnMedicineStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsImageSquare",
                table: "MedicinesStock",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsImageSquare",
                table: "MedicinesStock");
        }
    }
}
