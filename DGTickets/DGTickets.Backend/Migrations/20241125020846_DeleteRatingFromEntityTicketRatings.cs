using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DGTickets.Backend.Migrations
{
    /// <inheritdoc />
    public partial class DeleteRatingFromEntityTicketRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Ratings_RatingId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_RatingId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Tickets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RatingId",
                table: "Tickets",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Ratings_RatingId",
                table: "Tickets",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
