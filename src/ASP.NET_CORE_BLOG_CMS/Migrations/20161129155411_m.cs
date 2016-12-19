using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP.NET_CORE_BLOG_CMS.Migrations
{
    public partial class m : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "headerImageThumbnailUrl",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "headerImageThumbnailData",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "headerImageThumbnailData",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "headerImageThumbnailUrl",
                table: "Posts",
                nullable: true);
        }
    }
}
