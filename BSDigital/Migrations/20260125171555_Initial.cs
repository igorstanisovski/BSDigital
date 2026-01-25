using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BSDigital.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ORDER_BOOK_SNAPSHOT",
                columns: table => new
                {
                    ORDER_BOOK_SNAPSHOT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CREATED_ON = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CODE = table.Column<string>(type: "text", nullable: false),
                    BODY = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_BOOK_SNAPSHOT", x => x.ORDER_BOOK_SNAPSHOT_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORDER_BOOK_SNAPSHOT");
        }
    }
}
