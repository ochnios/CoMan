﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoMan.Data.Migrations
{
    public partial class AddCooperationRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CooperationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoinsiderationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Applicant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recipent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    ApplicantComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipentComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CooperationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CooperationRequests_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CooperationRequests_TopicId",
                table: "CooperationRequests",
                column: "TopicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CooperationRequests");
        }
    }
}
