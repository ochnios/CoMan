using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoMan.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MaxCooperations",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentLimit = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CooperationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ConsiderationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicantComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipentComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CooperationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CooperationRequests_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CooperationRequests_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CooperationRequests_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cooperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ConsiderationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mark = table.Column<float>(type: "real", nullable: true),
                    TopicId = table.Column<int>(type: "int", nullable: true),
                    CooperationRequestId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cooperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cooperations_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cooperations_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cooperations_CooperationRequests_CooperationRequestId",
                        column: x => x.CooperationRequestId,
                        principalTable: "CooperationRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cooperations_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CooperationRequests_StudentId",
                table: "CooperationRequests",
                column: "StudentId",
                unique: true,
                filter: "[StudentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CooperationRequests_TeacherId",
                table: "CooperationRequests",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CooperationRequests_TopicId",
                table: "CooperationRequests",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Cooperations_CooperationRequestId",
                table: "Cooperations",
                column: "CooperationRequestId",
                unique: true,
                filter: "[CooperationRequestId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cooperations_StudentId",
                table: "Cooperations",
                column: "StudentId",
                unique: true,
                filter: "[StudentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cooperations_TeacherId",
                table: "Cooperations",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Cooperations_TopicId",
                table: "Cooperations",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_AuthorId",
                table: "Topics",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cooperations");

            migrationBuilder.DropTable(
                name: "CooperationRequests");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxCooperations",
                table: "AspNetUsers");
        }
    }
}
