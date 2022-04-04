using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace InvestTeam.AutoBox.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => new { x.Key, x.Value });
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Commandlet = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Parameters = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    User = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    ObjectType = table.Column<int>(type: "int", nullable: false),
                    ObjectID = table.Column<int>(type: "int", nullable: false),
                    ObjectIdentity = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Source = table.Column<int>(type: "int", nullable: false),
                    RunID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vechicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identity = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vechicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Errors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryID = table.Column<int>(type: "int", nullable: false),
                    ErrorType = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    ErrorText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Errors_History_HistoryID",
                        column: x => x.HistoryID,
                        principalTable: "History",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoryDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Information = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Warning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryDetails_History_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "History",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    HistoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestLogs_History_HistoryID",
                        column: x => x.HistoryID,
                        principalTable: "History",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identity = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    VechicleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    SequenceNumber = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Vechicles_VechicleId",
                        column: x => x.VechicleId,
                        principalTable: "Vechicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Errors_HistoryID",
                table: "Errors",
                column: "HistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryDetails_HistoryId",
                table: "HistoryDetails",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Identity",
                table: "Orders",
                column: "Identity",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VechicleId",
                table: "Orders",
                column: "VechicleId");

            migrationBuilder.CreateIndex(
                name: "IX_RestLogs_HistoryID",
                table: "RestLogs",
                column: "HistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Vechicles_Identity",
                table: "Vechicles",
                column: "Identity",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "Errors");

            migrationBuilder.DropTable(
                name: "HistoryDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "RestLogs");

            migrationBuilder.DropTable(
                name: "Vechicles");

            migrationBuilder.DropTable(
                name: "History");
        }
    }
}
