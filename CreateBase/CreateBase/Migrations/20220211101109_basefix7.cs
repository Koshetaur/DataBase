using Microsoft.EntityFrameworkCore.Migrations;

namespace CreateBase.Migrations
{
    public partial class basefix7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservs_Rooms_RoomId",
                table: "Reservs");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservs_Users_UserId",
                table: "Reservs");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Reservs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Reservs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservs_Rooms_RoomId",
                table: "Reservs",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservs_Users_UserId",
                table: "Reservs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservs_Rooms_RoomId",
                table: "Reservs");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservs_Users_UserId",
                table: "Reservs");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Reservs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Reservs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

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
    }
}
