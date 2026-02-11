using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLTS.Infrastructure.Data.migration
{
    /// <inheritdoc />
    public partial class in2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeptName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeptName",
                table: "Employees");
        }
    }
}
