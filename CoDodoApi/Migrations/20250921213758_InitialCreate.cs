using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoDodoApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Opportunity",
                columns: table => new
                {
                    UriForAssignment = table.Column<string>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    Capability = table.Column<string>(type: "TEXT", nullable: false),
                    NameOfSalesLead = table.Column<string>(type: "TEXT", nullable: false),
                    HourlyRateInSEK = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opportunity", x => x.UriForAssignment);
                });

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FK_OpportunityUriForAssignment = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Processes_Opportunity_FK_OpportunityUriForAssignment",
                        column: x => x.FK_OpportunityUriForAssignment,
                        principalTable: "Opportunity",
                        principalColumn: "UriForAssignment",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Processes_FK_OpportunityUriForAssignment",
                table: "Processes",
                column: "FK_OpportunityUriForAssignment");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_Name_FK_OpportunityUriForAssignment",
                table: "Processes",
                columns: new[] { "Name", "FK_OpportunityUriForAssignment" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropTable(
                name: "Opportunity");
        }
    }
}
