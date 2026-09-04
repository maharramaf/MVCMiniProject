using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MVCMiniProject.Data;
using System;

#nullable disable

namespace MVCMiniProject.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260904120000_AuthVerificationDb")]
    public partial class AuthVerificationDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.AppUsers', 'VerificationExpiresAt') IS NULL
    ALTER TABLE [dbo].[AppUsers] ADD [VerificationExpiresAt] datetime2 NULL;
IF COL_LENGTH('dbo.AppUsers', 'LastVerificationSentAt') IS NULL
    ALTER TABLE [dbo].[AppUsers] ADD [LastVerificationSentAt] datetime2 NULL;
");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "AppUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AppUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = N'IX_AppUsers_Email' AND object_id = OBJECT_ID(N'dbo.AppUsers'))
BEGIN
    ;WITH d AS (
        SELECT Id, ROW_NUMBER() OVER (PARTITION BY Email ORDER BY Id) AS rn
        FROM AppUsers
    )
    UPDATE a SET Email = LEFT(a.Email, 230) + N'.dup' + CAST(a.Id AS nvarchar(20))
    FROM AppUsers a
    INNER JOIN d ON d.Id = a.Id
    WHERE d.rn > 1;

    CREATE UNIQUE INDEX [IX_AppUsers_Email] ON [dbo].[AppUsers]([Email]);
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUsers_Email",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "VerificationExpiresAt",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "LastVerificationSentAt",
                table: "AppUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
