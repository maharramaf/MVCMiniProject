using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCMiniProject.Migrations
{
    /// <inheritdoc />
    public partial class EmailDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "AppUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "AppUsers");
        }
    }
}
