using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saga.Infrastructure.Migrations
{
    public partial class i1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CargoStateInstance",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoStateInstance", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "CargoRouteInstance",
                columns: table => new
                {
                    CargoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Route = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoStateInstanceCorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoRouteInstance", x => x.CargoId);
                    table.ForeignKey(
                        name: "FK_CargoRouteInstance_CargoStateInstance_CargoStateInstanceCorrelationId",
                        column: x => x.CargoStateInstanceCorrelationId,
                        principalTable: "CargoStateInstance",
                        principalColumn: "CorrelationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CargoRouteInstance_CargoStateInstanceCorrelationId",
                table: "CargoRouteInstance",
                column: "CargoStateInstanceCorrelationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CargoRouteInstance");

            migrationBuilder.DropTable(
                name: "CargoStateInstance");
        }
    }
}
