using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoMan.Data.Migrations
{
    public partial class AddCommentsToCooperations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_AspNetUsers_StudentUserId",
                table: "Topics");

            migrationBuilder.RenameColumn(
                name: "StudentUserId",
                table: "Topics",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_StudentUserId",
                table: "Topics",
                newName: "IX_Topics_ApplicationUserId");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CooperationId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Cooperations_CooperationId",
                        column: x => x.CooperationId,
                        principalTable: "Cooperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CooperationId",
                table: "Comments",
                column: "CooperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_AspNetUsers_ApplicationUserId",
                table: "Topics",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_AspNetUsers_ApplicationUserId",
                table: "Topics");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Topics",
                newName: "StudentUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_ApplicationUserId",
                table: "Topics",
                newName: "IX_Topics_StudentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_AspNetUsers_StudentUserId",
                table: "Topics",
                column: "StudentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
