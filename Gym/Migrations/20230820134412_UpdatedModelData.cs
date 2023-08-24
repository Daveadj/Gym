using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModelData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "YearOfBirth",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DayOfBirth",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearOfBirth",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
