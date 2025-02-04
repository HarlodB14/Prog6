using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace beestjeopjefeestje.Migrations
{
    /// <inheritdoc />
    public partial class changedAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Accounts",
                newName: "Street_Name");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Accounts",
                newName: "Last_Name");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "First_Name",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Street_Number",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "First_Name",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Street_Number",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "Street_Name",
                table: "Accounts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Last_Name",
                table: "Accounts",
                newName: "Address");
        }
    }
}
