using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoMan.Data.Migrations
{
    public partial class MoveCooperationsToApplicationsUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cooperations_StudentId",
                table: "Cooperations");

            migrationBuilder.DropIndex(
                name: "IX_CooperationRequests_StudentId",
                table: "CooperationRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Topics",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "StudentUserId",
                table: "Topics",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_StudentUserId",
                table: "Topics",
                column: "StudentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cooperations_StudentId",
                table: "Cooperations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CooperationRequests_StudentId",
                table: "CooperationRequests",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_AspNetUsers_StudentUserId",
                table: "Topics",
                column: "StudentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_AspNetUsers_StudentUserId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_StudentUserId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Cooperations_StudentId",
                table: "Cooperations");

            migrationBuilder.DropIndex(
                name: "IX_CooperationRequests_StudentId",
                table: "CooperationRequests");

            migrationBuilder.DropColumn(
                name: "StudentUserId",
                table: "Topics");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_Cooperations_StudentId",
                table: "Cooperations",
                column: "StudentId",
                unique: true,
                filter: "[StudentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CooperationRequests_StudentId",
                table: "CooperationRequests",
                column: "StudentId",
                unique: true,
                filter: "[StudentId] IS NOT NULL");
        }
    }
}
