using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace beestjeopjefeestje.Migrations
{
    /// <inheritdoc />
    public partial class addedTotalDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalDiscount",
                table: "Accounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDiscount",
                table: "Accounts");
        }
    }
}
