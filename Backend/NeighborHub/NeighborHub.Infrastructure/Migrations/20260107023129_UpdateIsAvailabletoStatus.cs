using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeighborHub.Infrastructure.Migrations;

/// <inheritdoc />
public partial class UpdateIsAvailabletoStatus : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsAvailable",
            table: "Resources");

        migrationBuilder.AddColumn<int>(
            name: "Status",
            table: "Resources",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Status",
            table: "Resources");

        migrationBuilder.AddColumn<bool>(
            name: "IsAvailable",
            table: "Resources",
            type: "bit",
            nullable: false,
            defaultValue: true);
    }
}
