using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMP.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ApplicationId",
                table: "Notifications",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_JobSeekerId",
                table: "Notifications",
                column: "JobSeekerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactRequests_ApplicationId",
                table: "ContactRequests",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactRequests_EmployerId",
                table: "ContactRequests",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactRequests_JobSeekerId",
                table: "ContactRequests",
                column: "JobSeekerId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobVacancyId",
                table: "Applications",
                column: "JobVacancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobVacancies_JobVacancyId",
                table: "Applications",
                column: "JobVacancyId",
                principalTable: "JobVacancies",
                principalColumn: "JobVacancyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_JobSeekerId",
                table: "Applications",
                column: "JobSeekerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactRequests_Applications_ApplicationId",
                table: "ContactRequests",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactRequests_Users_EmployerId",
                table: "ContactRequests",
                column: "EmployerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactRequests_Users_JobSeekerId",
                table: "ContactRequests",
                column: "JobSeekerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Applications_ApplicationId",
                table: "Notifications",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_JobSeekerId",
                table: "Notifications",
                column: "JobSeekerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobVacancies_JobVacancyId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_JobSeekerId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactRequests_Applications_ApplicationId",
                table: "ContactRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactRequests_Users_EmployerId",
                table: "ContactRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactRequests_Users_JobSeekerId",
                table: "ContactRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Applications_ApplicationId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_JobSeekerId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ApplicationId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_JobSeekerId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_ContactRequests_ApplicationId",
                table: "ContactRequests");

            migrationBuilder.DropIndex(
                name: "IX_ContactRequests_EmployerId",
                table: "ContactRequests");

            migrationBuilder.DropIndex(
                name: "IX_ContactRequests_JobSeekerId",
                table: "ContactRequests");

            migrationBuilder.DropIndex(
                name: "IX_Applications_JobVacancyId",
                table: "Applications");
        }
    }
}
