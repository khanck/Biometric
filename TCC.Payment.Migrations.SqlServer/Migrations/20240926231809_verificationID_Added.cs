using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCC.Payment.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class verificationID_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "verificationID",
                table: "BiometricVerifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "verificationID",
                table: "BiometricVerifications");
        }
    }
}
