using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeighborHub.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Initialmigrate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Resources",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Resources", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Resources");
    }
}
