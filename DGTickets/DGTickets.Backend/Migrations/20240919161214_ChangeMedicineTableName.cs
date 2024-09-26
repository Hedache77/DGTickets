using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DGTickets.Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMedicineTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicineStocks",
                table: "MedicineStocks");

            migrationBuilder.RenameTable(
                name: "MedicineStocks",
                newName: "MedicinesStock");

            migrationBuilder.RenameIndex(
                name: "IX_MedicineStocks_Name",
                table: "MedicinesStock",
                newName: "IX_MedicinesStock_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicinesStock",
                table: "MedicinesStock",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicinesStock",
                table: "MedicinesStock");

            migrationBuilder.RenameTable(
                name: "MedicinesStock",
                newName: "MedicineStocks");

            migrationBuilder.RenameIndex(
                name: "IX_MedicinesStock_Name",
                table: "MedicineStocks",
                newName: "IX_MedicineStocks_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicineStocks",
                table: "MedicineStocks",
                column: "Id");
        }
    }
}
