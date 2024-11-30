using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiveMinute.Migrations
{
    /// <inheritdoc />
    public partial class RENAMINGABAYUDNA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FiveMinuteTests_FiveMinuteTemplates_AttachedFMTId",
                table: "FiveMinuteTests");

            migrationBuilder.RenameColumn(
                name: "AttachedFMTId",
                table: "FiveMinuteTests",
                newName: "FiveMinuteTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_FiveMinuteTests_AttachedFMTId",
                table: "FiveMinuteTests",
                newName: "IX_FiveMinuteTests_FiveMinuteTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_FiveMinuteTests_FiveMinuteTemplates_FiveMinuteTemplateId",
                table: "FiveMinuteTests",
                column: "FiveMinuteTemplateId",
                principalTable: "FiveMinuteTemplates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FiveMinuteTests_FiveMinuteTemplates_FiveMinuteTemplateId",
                table: "FiveMinuteTests");

            migrationBuilder.RenameColumn(
                name: "FiveMinuteTemplateId",
                table: "FiveMinuteTests",
                newName: "AttachedFMTId");

            migrationBuilder.RenameIndex(
                name: "IX_FiveMinuteTests_FiveMinuteTemplateId",
                table: "FiveMinuteTests",
                newName: "IX_FiveMinuteTests_AttachedFMTId");

            migrationBuilder.AddForeignKey(
                name: "FK_FiveMinuteTests_FiveMinuteTemplates_AttachedFMTId",
                table: "FiveMinuteTests",
                column: "AttachedFMTId",
                principalTable: "FiveMinuteTemplates",
                principalColumn: "Id");
        }
    }
}
