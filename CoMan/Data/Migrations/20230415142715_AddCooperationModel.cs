using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoMan.Data.Migrations
{
    public partial class AddCooperationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CooperationId",
                table: "CooperationRequests",
                type: "int",
                nullable: true);

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
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    Student = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Teacher = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cooperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cooperations_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CooperationRequests_CooperationId",
                table: "CooperationRequests",
                column: "CooperationId",
                unique: true,
                filter: "[CooperationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cooperations_TopicId",
                table: "Cooperations",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_CooperationRequests_Cooperations_CooperationId",
                table: "CooperationRequests",
                column: "CooperationId",
                principalTable: "Cooperations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CooperationRequests_Cooperations_CooperationId",
                table: "CooperationRequests");

            migrationBuilder.DropTable(
                name: "Cooperations");

            migrationBuilder.DropIndex(
                name: "IX_CooperationRequests_CooperationId",
                table: "CooperationRequests");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "CooperationId",
                table: "CooperationRequests");
        }
    }
}
