using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class chat_entities_attributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ChatRoom",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "ChatMessage",
                newName: "message");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "ChatRoom",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "ChatRoom",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "ChatMessage",
                newName: "Message");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ChatRoom",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
