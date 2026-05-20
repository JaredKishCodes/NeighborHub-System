using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeighborHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class additemheadphone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/item-images/mountain_bike.jpg");

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/item-images/drill_set.jpg");

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "ImageUrl", "Name", "OwnerId" },
                values: new object[] { new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "ya'll can borrow this, cause i just got my new one.", "/item-images/headphone.jpg", "Headphone", 3 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "ImageUrl", "ItemStatus", "LastUpdatedAt", "Name", "OwnerId" },
                values: new object[] { 1, "Tools", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "very good hammer, slightly used.", "/item-images/hammer.jpg", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hammer", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/item-images/mountainbike.jpg");

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/item-images/drillset.jpg");

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "ImageUrl", "Name", "OwnerId" },
                values: new object[] { new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "very good hammer, slightly used.", "/item-images/hammer.jpg", "Hammer", 2 });
        }
    }
}
