using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchMonolit.Infrastructure.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "profiles",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "profiles",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "users");

            migrationBuilder.UpdateData(
                table: "profiles",
                keyColumn: "id",
                keyValue: 2,
                column: "profile_name",
                value: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "profiles",
                keyColumn: "id",
                keyValue: 2,
                column: "profile_name",
                value: "CompanyOwner");

            migrationBuilder.InsertData(
                table: "profiles",
                columns: new[] { "id", "profile_name" },
                values: new object[,]
                {
                    { 3, "User" },
                    { 4, "Employee" }
                });
        }
    }
}
