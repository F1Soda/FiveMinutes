using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiveMinute.Migrations
{
    /// <inheritdoc />
    public partial class MinorChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "UserAnswer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FiveMinuteResults_FiveMinuteTemplateId",
                table: "FiveMinuteResults",
                column: "FiveMinuteTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_FiveMinuteResults_FiveMinuteTemplates_FiveMinuteTemplateId",
                table: "FiveMinuteResults",
                column: "FiveMinuteTemplateId",
                principalTable: "FiveMinuteTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FiveMinuteResults_FiveMinuteTemplates_FiveMinuteTemplateId",
                table: "FiveMinuteResults");

            migrationBuilder.DropIndex(
                name: "IX_FiveMinuteResults_FiveMinuteTemplateId",
                table: "FiveMinuteResults");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "UserAnswer");
        }
    }
}
