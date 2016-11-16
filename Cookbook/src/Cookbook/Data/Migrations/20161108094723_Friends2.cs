using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cookbook.Data.Migrations
{
    public partial class Friends2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_FriendRequests_RequestId",
                table: "Friend");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_AspNetUsers_UserId",
                table: "Friend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friend",
                table: "Friend");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friends",
                table: "Friend",
                columns: new[] { "UserId", "RequestId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_FriendRequests_RequestId",
                table: "Friend",
                column: "RequestId",
                principalTable: "FriendRequests",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_UserId",
                table: "Friend",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameIndex(
                name: "IX_Friend_UserId",
                table: "Friend",
                newName: "IX_Friends_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_RequestId",
                table: "Friend",
                newName: "IX_Friends_RequestId");

            migrationBuilder.RenameTable(
                name: "Friend",
                newName: "Friends");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_FriendRequests_RequestId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_UserId",
                table: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friends",
                table: "Friends");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friend",
                table: "Friends",
                columns: new[] { "UserId", "RequestId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_FriendRequests_RequestId",
                table: "Friends",
                column: "RequestId",
                principalTable: "FriendRequests",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_AspNetUsers_UserId",
                table: "Friends",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameIndex(
                name: "IX_Friends_UserId",
                table: "Friends",
                newName: "IX_Friend_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_RequestId",
                table: "Friends",
                newName: "IX_Friend_RequestId");

            migrationBuilder.RenameTable(
                name: "Friends",
                newName: "Friend");
        }
    }
}
