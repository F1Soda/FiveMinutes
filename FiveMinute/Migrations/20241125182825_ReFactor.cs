using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiveMinute.Migrations
{
    /// <inheritdoc />
    public partial class ReFactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FiveMinuteTests_AspNetUsers_AppUserId",
                table: "FiveMinuteTests");

            migrationBuilder.DropIndex(
                name: "IX_FiveMinuteTests_AppUserId",
                table: "FiveMinuteTests");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "FiveMinuteTests");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "FiveMinuteResults");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "FiveMinuteResults",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FiveMinuteResults_AppUserId",
                table: "FiveMinuteResults",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FiveMinuteResults_AspNetUsers_AppUserId",
                table: "FiveMinuteResults",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "FMTestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FiveMinuteResults_AspNetUsers_AppUserId",
                table: "FiveMinuteResults");

            migrationBuilder.DropIndex(
                name: "IX_FiveMinuteResults_AppUserId",
                table: "FiveMinuteResults");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "FiveMinuteResults");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "FiveMinuteTests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "FiveMinuteResults",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FiveMinuteTests_AppUserId",
                table: "FiveMinuteTests",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FiveMinuteTests_AspNetUsers_AppUserId",
                table: "FiveMinuteTests",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "FMTestId");
        }
    }
}
