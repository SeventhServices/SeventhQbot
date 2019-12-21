using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SeventhServices.QQRobot.Resource.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountBindings",
                columns: table => new
                {
                    Qq = table.Column<string>(nullable: false),
                    BoundAccountPid = table.Column<string>(nullable: false),
                    BindTime = table.Column<DateTime>(nullable: false),
                    IsPidOnly = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountBindings", x => new { x.BoundAccountPid, x.Qq });
                });

            migrationBuilder.CreateTable(
                name: "BoundAccounts",
                columns: table => new
                {
                    Pid = table.Column<string>(nullable: false),
                    Qq = table.Column<string>(nullable: false),
                    Uuid = table.Column<string>(nullable: true),
                    EncPid = table.Column<string>(nullable: true),
                    Id = table.Column<string>(nullable: true),
                    Tpid = table.Column<string>(nullable: true),
                    Ivs = table.Column<string>(nullable: true),
                    EncUuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoundAccounts", x => new { x.Pid, x.Qq });
                    table.ForeignKey(
                        name: "FK_BoundAccounts_AccountBindings_Pid_Qq",
                        columns: x => new { x.Pid, x.Qq },
                        principalTable: "AccountBindings",
                        principalColumns: new[] { "BoundAccountPid", "Qq" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoundAccounts");

            migrationBuilder.DropTable(
                name: "AccountBindings");
        }
    }
}
