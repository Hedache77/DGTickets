using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DGTickets.Backend.Migrations
{
    /// <inheritdoc />
    public partial class ImageFieldToMedicineStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MedicineStocks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "MedicineStocks");
        }
    }
}
