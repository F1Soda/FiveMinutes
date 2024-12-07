using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiveMinute.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "FiveMinuteResults",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "StudentData_FirstName",
                table: "FiveMinuteResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentData_Group",
                table: "FiveMinuteResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentData_LastName",
                table: "FiveMinuteResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentData_FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentData_Group",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentData_LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentData_FirstName",
                table: "FiveMinuteResults");

            migrationBuilder.DropColumn(
                name: "StudentData_Group",
                table: "FiveMinuteResults");

            migrationBuilder.DropColumn(
                name: "StudentData_LastName",
                table: "FiveMinuteResults");

            migrationBuilder.DropColumn(
                name: "StudentData_FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudentData_Group",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudentData_LastName",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "FiveMinuteResults",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
