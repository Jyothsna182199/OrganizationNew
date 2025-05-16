using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizationForm.Migrations
{
    /// <inheritdoc />
    public partial class Form : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrgForm1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerExperience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegulatoryRegion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<int>(type: "int", nullable: false),
                    StorePrefix = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgForm1", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrgForm1");
        }
    }
}
