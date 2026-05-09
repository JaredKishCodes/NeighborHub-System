using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NeighborHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "ImageUrl", "ItemStatus", "LastUpdatedAt", "Name", "OwnerId" },
                values: new object[,]
                {
                    { 2, "Sports", new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Adult size, 21-speed mountain bike.", "/item-images/mountain_bike.jpg", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mountain Bike", 2 },
                    { 3, "Tools", new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Cordless drill with two batteries.", "/item-images/drill_set.jpg", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Drill Set", 3 },
                    { 4, "Tools", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "very good hammer, slightly used.", "/item-images/hammer.jpg", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hammer", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
