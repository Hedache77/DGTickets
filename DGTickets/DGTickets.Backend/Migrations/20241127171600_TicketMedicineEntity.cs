using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DGTickets.Backend.Migrations
{
    /// <inheritdoc />
    public partial class TicketMedicineEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                 name: "TicketMedicines",
                 columns: table => new
                 {
                     Id = table.Column<int>(type: "int", nullable: false)
                         .Annotation("SqlServer:Identity", "1, 1"),
                     TicketId = table.Column<int>(type: "int", nullable: false),
                     MedicineId = table.Column<int>(type: "int", nullable: false)
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_TicketMedicines", x => x.Id);
                     table.ForeignKey(
                         name: "FK_TicketMedicines_MedicinesStock_MedicineId",
                         column: x => x.MedicineId,
                         principalTable: "MedicinesStock",
                         principalColumn: "Id",
                         onDelete: ReferentialAction.Restrict);
                     table.ForeignKey(
                         name: "FK_TicketMedicines_Tickets_TicketId",
                         column: x => x.TicketId,
                         principalTable: "Tickets",
                         principalColumn: "Id",
                         onDelete: ReferentialAction.Restrict);
                 });

            migrationBuilder.CreateIndex(
                 name: "IX_TicketMedicines_MedicineId",
                 table: "TicketMedicines",
                 column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMedicines_TicketId_MedicineId",
                table: "TicketMedicines",
                columns: new[] { "TicketId", "MedicineId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                 name: "TicketMedicines");
        }
    }
}