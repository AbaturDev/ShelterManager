using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShelterManager.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAnimalAndAdoptionPersonEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Animals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Person_DocumentId",
                table: "Adoptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Person_Pesel",
                table: "Adoptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "Person_DocumentId",
                table: "Adoptions");

            migrationBuilder.DropColumn(
                name: "Person_Pesel",
                table: "Adoptions");
        }
    }
}
