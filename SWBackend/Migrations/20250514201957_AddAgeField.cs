using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddAgeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "PersonalDatas",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "PersonalDatas");
        }
    }
}
