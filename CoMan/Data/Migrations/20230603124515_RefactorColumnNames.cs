using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoMan.Data.Migrations
{
    public partial class RefactorColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Cooperations",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "ConsiderationDate",
                table: "Cooperations",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "RecipentComment",
                table: "CooperationRequests",
                newName: "TeacherComment");

            migrationBuilder.RenameColumn(
                name: "ApplicantComment",
                table: "CooperationRequests",
                newName: "StudentComment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Cooperations",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Cooperations",
                newName: "ConsiderationDate");

            migrationBuilder.RenameColumn(
                name: "TeacherComment",
                table: "CooperationRequests",
                newName: "RecipentComment");

            migrationBuilder.RenameColumn(
                name: "StudentComment",
                table: "CooperationRequests",
                newName: "ApplicantComment");
        }
    }
}
