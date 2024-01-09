using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NSSERP.Migrations
{
    /// <inheritdoc />
    public partial class AddCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COUNTRY_MASTER",
                columns: table => new
                {
                    Country_Code = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Country_Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Country_CallCode = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Data_Flag = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FY_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mob_Length = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency_id = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COUNTRY_MASTER", x => x.Country_Code);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COUNTRY_MASTER");
        }
    }
}
