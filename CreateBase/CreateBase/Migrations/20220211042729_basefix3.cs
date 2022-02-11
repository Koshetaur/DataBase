using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CreateBase.Migrations
{
    public partial class basefix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Reservs");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeEnd",
                table: "Reservs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStart",
                table: "Reservs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeEnd",
                table: "Reservs");

            migrationBuilder.DropColumn(
                name: "TimeStart",
                table: "Reservs");

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Reservs",
                type: "TEXT",
                nullable: true);
        }
    }
}
