namespace Domain.Interfaces;

public interface ITransaction
{
    string? ApprenticeName { get; set; }
    string? ApprenticeshipTrainingCourse { get; set; }
    int CourseLevel { get; set; }
    string Description { get; set; }
    decimal EnglishPercentage { get; set; }
    decimal GovernmentContribution { get; set; }
    Guid Id { get; set; }
    decimal LevyDeclared { get; set; }
    decimal PaidFromLevy { get; set; }
    string? PayeScheme { get; set; }
    DateTime PayrollMonth { get; set; }
    decimal TenPercentageTopUp { get; set; }
    decimal Total { get; set; }
    DateTime TransactionDate { get; set; }
    string TransactionType { get; set; }
    string? TrainingProvider { get; set; }
    decimal? ULN { get; set; }
    decimal YourContribution { get; set; }
}