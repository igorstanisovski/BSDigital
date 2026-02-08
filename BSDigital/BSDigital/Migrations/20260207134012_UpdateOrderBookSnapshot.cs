using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSDigital.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderBookSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ORDER_BOOK_SNAPSHOT_CREATED_ON",
                table: "ORDER_BOOK_SNAPSHOT",
                column: "CREATED_ON",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ORDER_BOOK_SNAPSHOT_CREATED_ON",
                table: "ORDER_BOOK_SNAPSHOT");
        }
    }
}
