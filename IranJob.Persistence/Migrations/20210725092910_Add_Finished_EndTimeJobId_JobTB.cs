using Microsoft.EntityFrameworkCore.Migrations;

namespace IranJob.Persistence.Migrations
{
    public partial class Add_Finished_EndTimeJobId_JobTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndTimeJobId",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Jobs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTimeJobId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Jobs");
        }
    }
}
