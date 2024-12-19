using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiveMinute.Migrations
{
    /// <inheritdoc />
    public partial class StyopaCantCreateButICan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "UserAnswer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Сost",
                table: "Question",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "FiveMinuteResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "UserAnswer");

            migrationBuilder.DropColumn(
                name: "Сost",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "FiveMinuteResults");
        }
    }
}
