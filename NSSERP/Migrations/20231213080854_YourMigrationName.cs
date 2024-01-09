using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NSSERP.Migrations
{
    /// <inheritdoc />
    public partial class YourMigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_COUNTRY_MASTER",
                table: "COUNTRY_MASTER");

            migrationBuilder.DropColumn(
                name: "Country_Code",
                table: "COUNTRY_MASTER");

            migrationBuilder.DropColumn(
                name: "Country_CallCode",
                table: "COUNTRY_MASTER");

            migrationBuilder.DropColumn(
                name: "Country_Name",
                table: "COUNTRY_MASTER");

            migrationBuilder.DropColumn(
                name: "Currency_id",
                table: "COUNTRY_MASTER");

            migrationBuilder.DropColumn(
                name: "Data_Flag",
                table: "COUNTRY_MASTER");

            migrationBuilder.DropColumn(
                name: "FY_ID",
                table: "COUNTRY_MASTER");

            migrationBuilder.DropColumn(
                name: "Mob_Length",
                table: "COUNTRY_MASTER");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "COUNTRY_MASTER");

            migrationBuilder.RenameTable(
                name: "COUNTRY_MASTER",
                newName: "CountryMaster");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "CountryMaster",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "CountryMaster",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "CountryMaster",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CountryMaster",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IsActive",
                table: "CountryMaster",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CountryMaster",
                table: "CountryMaster",
                column: "CountryId");

            migrationBuilder.CreateTable(
                name: "StateMaster",
                columns: table => new
                {
                    StateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateMaster", x => x.StateID);
                    table.ForeignKey(
                        name: "FK_StateMaster_CountryMaster_CountryID",
                        column: x => x.CountryID,
                        principalTable: "CountryMaster",
                        principalColumn: "CountryId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StateMaster_CountryID",
                table: "StateMaster",
                column: "CountryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StateMaster");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CountryMaster",
                table: "CountryMaster");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "CountryMaster");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "CountryMaster");

            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "CountryMaster");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CountryMaster");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CountryMaster");

            migrationBuilder.RenameTable(
                name: "CountryMaster",
                newName: "COUNTRY_MASTER");

            migrationBuilder.AddColumn<decimal>(
                name: "Country_Code",
                table: "COUNTRY_MASTER",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Country_CallCode",
                table: "COUNTRY_MASTER",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country_Name",
                table: "COUNTRY_MASTER",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Currency_id",
                table: "COUNTRY_MASTER",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Data_Flag",
                table: "COUNTRY_MASTER",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "FY_ID",
                table: "COUNTRY_MASTER",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mob_Length",
                table: "COUNTRY_MASTER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "COUNTRY_MASTER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_COUNTRY_MASTER",
                table: "COUNTRY_MASTER",
                column: "Country_Code");
        }
    }
}
