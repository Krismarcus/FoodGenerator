using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutritionApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FoodItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Calories = table.Column<double>(type: "double", nullable: false),
                    Fats_Total = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Fats_Omega3 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Fats_Omega6 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Fats_TransFat = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Proteins_Total = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Carbohydrates_Total = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Carbohydrates_Fiber = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Carbohydrates_Sugar = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Carbohydrates_ComplexCarbs = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Weight = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItems", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StorageItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Weight = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageItems", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodItems");

            migrationBuilder.DropTable(
                name: "StorageItems");
        }
    }
}
