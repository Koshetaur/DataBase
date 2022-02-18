using Microsoft.EntityFrameworkCore.Migrations;

namespace CreateBase.Migrations
{
    public partial class basefix9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "RoomName_Index",
                table: "Rooms",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "RoomName_Index",
                table: "Rooms");
        }
    }
}
