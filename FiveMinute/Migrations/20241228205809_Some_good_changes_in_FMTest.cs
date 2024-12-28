using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiveMinute.Migrations
{
    /// <inheritdoc />
    public partial class Some_good_changes_in_FMTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EndPlanned",
                table: "FiveMinuteTests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StartPlanned",
                table: "FiveMinuteTests",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndPlanned",
                table: "FiveMinuteTests");

            migrationBuilder.DropColumn(
                name: "StartPlanned",
                table: "FiveMinuteTests");
        }
    }
}
