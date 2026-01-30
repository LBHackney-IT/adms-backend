using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apprentices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApprenticeAchievement = table.Column<string>(type: "text", nullable: true),
                    ApprenticeConfirmation = table.Column<string>(type: "text", nullable: true),
                    ApprenticeClassification = table.Column<string>(type: "text", nullable: true),
                    ApprenticeEthnicity = table.Column<string>(type: "text", nullable: true),
                    ApprenticeGender = table.Column<string>(type: "text", nullable: true),
                    ApprenticeNonCompletionReason = table.Column<string>(type: "text", nullable: true),
                    ApprenticeProgram = table.Column<string>(type: "text", nullable: true),
                    ApprenticeProgression = table.Column<string>(type: "text", nullable: true),
                    ApprenticeshipDelivery = table.Column<string>(type: "text", nullable: true),
                    CertificatesReceived = table.Column<string>(type: "text", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Directorate = table.Column<string>(type: "text", nullable: true),
                    DoeReference = table.Column<string>(type: "text", nullable: true),
                    EmployeeNumber = table.Column<string>(type: "text", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndPointAssessor = table.Column<string>(type: "text", nullable: true),
                    IsCareLeaver = table.Column<bool>(type: "boolean", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    ManagerName = table.Column<string>(type: "text", nullable: true),
                    ManagerTitle = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PauseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Post = table.Column<string>(type: "text", nullable: true),
                    School = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TotalAgreedApprenticeshipPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TrainingCourse = table.Column<string>(type: "text", nullable: true),
                    TrainingProvider = table.Column<string>(type: "text", nullable: true),
                    UKPRN = table.Column<decimal>(type: "numeric", nullable: true),
                    ULN = table.Column<decimal>(type: "numeric", nullable: false),
                    WithdrawalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apprentices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Details = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    EventTypeTargetId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApprenticeName = table.Column<string>(type: "text", nullable: true),
                    ApprenticeshipTrainingCourse = table.Column<string>(type: "text", nullable: true),
                    CourseLevel = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    EnglishPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    GovernmentContribution = table.Column<decimal>(type: "numeric", nullable: false),
                    LevyDeclared = table.Column<decimal>(type: "numeric", nullable: false),
                    PaidFromLevy = table.Column<decimal>(type: "numeric", nullable: false),
                    PayeScheme = table.Column<string>(type: "text", nullable: true),
                    PayrollMonth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenPercentageTopUp = table.Column<decimal>(type: "numeric", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionType = table.Column<string>(type: "text", nullable: false),
                    TrainingProvider = table.Column<string>(type: "text", nullable: true),
                    ULN = table.Column<decimal>(type: "numeric", nullable: true),
                    YourContribution = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apprentices_ULN",
                table: "Apprentices",
                column: "ULN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apprentices");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
