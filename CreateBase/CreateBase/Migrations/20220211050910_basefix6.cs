using Microsoft.EntityFrameworkCore.Migrations;

namespace CreateBase.Migrations
{
    public partial class basefix6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdRoom",
                table: "Reservs");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Reservs");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Reservs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Reservs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservs_RoomId",
                table: "Reservs",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservs_UserId",
                table: "Reservs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservs_Rooms_RoomId",
                table: "Reservs",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservs_Users_UserId",
                table: "Reservs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservs_Rooms_RoomId",
                table: "Reservs");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservs_Users_UserId",
                table: "Reservs");

            migrationBuilder.DropIndex(
                name: "IX_Reservs_RoomId",
                table: "Reservs");

            migrationBuilder.DropIndex(
                name: "IX_Reservs_UserId",
                table: "Reservs");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Reservs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reservs");

            migrationBuilder.AddColumn<int>(
                name: "IdRoom",
                table: "Reservs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUser",
                table: "Reservs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
