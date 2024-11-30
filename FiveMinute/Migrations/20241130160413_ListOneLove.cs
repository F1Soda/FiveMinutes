using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiveMinute.Migrations
{
    /// <inheritdoc />
    public partial class ListOneLove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FiveMinuteResults_FiveMinuteTemplates_FiveMinuteTemplateId",
                table: "FiveMinuteResults");

            migrationBuilder.DropForeignKey(
                name: "FK_FiveMinuteResults_FiveMinuteTests_FiveMinuteTestId",
                table: "FiveMinuteResults");

            migrationBuilder.DropIndex(
                name: "IX_FiveMinuteResults_FiveMinuteTemplateId",
                table: "FiveMinuteResults");

            migrationBuilder.DropColumn(
                name: "FiveMinuteTemplateId",
                table: "FiveMinuteResults");

            migrationBuilder.AlterColumn<int>(
                name: "FiveMinuteTestId",
                table: "FiveMinuteResults",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FiveMinuteResults_FiveMinuteTests_FiveMinuteTestId",
                table: "FiveMinuteResults",
                column: "FiveMinuteTestId",
                principalTable: "FiveMinuteTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FiveMinuteResults_FiveMinuteTests_FiveMinuteTestId",
                table: "FiveMinuteResults");

            migrationBuilder.AlterColumn<int>(
                name: "FiveMinuteTestId",
                table: "FiveMinuteResults",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "FiveMinuteTemplateId",
                table: "FiveMinuteResults",
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

            migrationBuilder.AddForeignKey(
                name: "FK_FiveMinuteResults_FiveMinuteTests_FiveMinuteTestId",
                table: "FiveMinuteResults",
                column: "FiveMinuteTestId",
                principalTable: "FiveMinuteTests",
                principalColumn: "Id");
        }
    }
}
