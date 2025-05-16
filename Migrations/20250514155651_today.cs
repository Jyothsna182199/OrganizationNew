using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizationForm.Migrations
{
    /// <inheritdoc />
    public partial class today : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UploadedFileName",
                table: "OrgForm1",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UploadedFileUrl",
                table: "OrgForm1",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadedFileName",
                table: "OrgForm1");

            migrationBuilder.DropColumn(
                name: "UploadedFileUrl",
                table: "OrgForm1");
        }
    }
}
