using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EF11_EF_Migration2.Migrations
{
    /// <inheritdoc />
    public partial class addofficeentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    OfficeName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    OfficeLocation = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    LName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instructors_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseName", "Price" },
                values: new object[,]
                {
                    { 1, "Mathematics", 1000m },
                    { 2, "Physics", 2000m },
                    { 3, "Chemistry", 1500m },
                    { 4, "Biology", 1200m },
                    { 5, "CS-50", 3000m }
                });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "OfficeLocation", "OfficeName" },
                values: new object[,]
                {
                    { 1, "Building A", "Off_05" },
                    { 2, "Building B", "Off_12" },
                    { 3, "Administration", "Off_32" },
                    { 4, "IT Department", "Off_44" },
                    { 5, "IT Department", "Off_43" }
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "FName", "LName", "OfficeId" },
                values: new object[,]
                {
                    { 1, "Ahmed", "Abdullah", 1 },
                    { 2, "Yasmeen", "Mohammed", 2 },
                    { 3, "Khalid", "Hassan", 3 },
                    { 4, "Nadia", "Ali", 4 },
                    { 5, "Omar", "Ibrahim", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_OfficeId",
                table: "Instructors",
                column: "OfficeId",
                unique: true,
                filter: "[OfficeId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}
