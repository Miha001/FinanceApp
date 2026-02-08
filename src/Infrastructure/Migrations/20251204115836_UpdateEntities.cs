using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_tokens_UserId",
                table: "user_tokens");

            migrationBuilder.CreateIndex(
                name: "IX_users_Name",
                table: "users",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_tokens_UserId",
                table: "user_tokens",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_Name",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_user_tokens_UserId",
                table: "user_tokens");

            migrationBuilder.CreateIndex(
                name: "IX_user_tokens_UserId",
                table: "user_tokens",
                column: "UserId");
        }
    }
}
