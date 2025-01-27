using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace beestjeopjefeestje.Migrations
{
    /// <inheritdoc />
    public partial class addedmiddleNameAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Middle_Name",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Middle_Name",
                table: "Accounts");
        }
    }
}
