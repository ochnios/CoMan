using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoMan.Data.Migrations
{
    public partial class AddDeletedFlagToModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Topics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Cooperations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "CooperationRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Cooperations");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "CooperationRequests");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "AspNetUsers");
        }
    }
}
