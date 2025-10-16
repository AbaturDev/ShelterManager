using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShelterManager.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedAdoptionEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adoptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartAdoptionProcess = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AdoptionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Person_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Person_Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Person_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Person_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Person_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Person_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Person_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnimalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adoptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adoptions_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adoptions_AnimalId",
                table: "Adoptions",
                column: "AnimalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adoptions");
        }
    }
}
