using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMP.Migrations
{
    /// <inheritdoc />
    public partial class AddRequiredEducation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequiredEducation",
                table: "JobVacancies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeekerCvs_UserId",
                table: "JobSeekerCvs",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerCvs_Users_UserId",
                table: "JobSeekerCvs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerCvs_Users_UserId",
                table: "JobSeekerCvs");

            migrationBuilder.DropIndex(
                name: "IX_JobSeekerCvs_UserId",
                table: "JobSeekerCvs");

            migrationBuilder.DropColumn(
                name: "RequiredEducation",
                table: "JobVacancies");
        }
    }
}
