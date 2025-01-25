using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace beestjeopjefeestje.Migrations
{
    /// <inheritdoc />
    public partial class linkanimalbooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDate",
                table: "Animals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Animals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "Animals",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animals_BookingId",
                table: "Animals",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Bookings_BookingId",
                table: "Animals",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Bookings_BookingId",
                table: "Animals");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Animals_BookingId",
                table: "Animals");

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
