using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeighborHub.Infrastructure.Migrations;

public partial class AddChatMessages : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "OverdueNotified",
            table: "Booking",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.CreateTable(
            name: "ChatMessage",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ParticipantOneId = table.Column<int>(type: "int", nullable: false),
                ParticipantTwoId = table.Column<int>(type: "int", nullable: false),
                SenderId = table.Column<int>(type: "int", nullable: true),
                RecipientId = table.Column<int>(type: "int", nullable: false),
                Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                MessageType = table.Column<int>(type: "int", nullable: false),
                SystemEventType = table.Column<int>(type: "int", nullable: true),
                BookingId = table.Column<int>(type: "int", nullable: true),
                SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsRead = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ChatMessage", x => x.Id);
                table.ForeignKey(
                    name: "FK_ChatMessage_DomainUsers_RecipientId",
                    column: x => x.RecipientId,
                    principalTable: "DomainUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ChatMessage_DomainUsers_SenderId",
                    column: x => x.SenderId,
                    principalTable: "DomainUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessage_ParticipantOneId_ParticipantTwoId_SentAt",
            table: "ChatMessage",
            columns: new[] { "ParticipantOneId", "ParticipantTwoId", "SentAt" });

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessage_RecipientId",
            table: "ChatMessage",
            column: "RecipientId");

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessage_SenderId",
            table: "ChatMessage",
            column: "SenderId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "ChatMessage");

        migrationBuilder.DropColumn(
            name: "OverdueNotified",
            table: "Booking");
    }
}
