using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace beestjeopjefeestje.Migrations
{
    /// <inheritdoc />
    public partial class changedbookingandanimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add columns for Animal model
            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDate",
                table: "Animals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Animals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "Animals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Create the Booking table
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    SelectedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountOfAnimals = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            // If needed, add foreign key from Animal to Booking
            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Bookings_BookingId",
                table: "Animals",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key if it was added
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Bookings_BookingId",
                table: "Animals");

            // Drop Booking table
            migrationBuilder.DropTable(
                name: "Bookings");

            // Drop columns for Animal model
            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "Animals");
        }
    }
}
