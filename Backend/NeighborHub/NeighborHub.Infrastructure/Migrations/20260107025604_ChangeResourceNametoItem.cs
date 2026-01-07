using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeighborHub.Infrastructure.Migrations;

/// <inheritdoc />
public partial class ChangeResourceNametoItem : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Resources");

        migrationBuilder.CreateTable(
            name: "Items",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Items", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Items");

        migrationBuilder.CreateTable(
            name: "Resources",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Resources", x => x.Id);
            });
    }
}
