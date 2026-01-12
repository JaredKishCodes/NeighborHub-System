using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeighborHub.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddNewChanges : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "OwnerId",
            table: "Items",
            type: "int",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.CreateIndex(
            name: "IX_Items_OwnerId",
            table: "Items",
            column: "OwnerId");

        migrationBuilder.AddForeignKey(
            name: "FK_Items_DomainUsers_OwnerId",
            table: "Items",
            column: "OwnerId",
            principalTable: "DomainUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Items_DomainUsers_OwnerId",
            table: "Items");

        migrationBuilder.DropIndex(
            name: "IX_Items_OwnerId",
            table: "Items");

        migrationBuilder.AlterColumn<string>(
            name: "OwnerId",
            table: "Items",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");
    }
}
