using System;
using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;

namespace Domain.Entities;

public class Transaction: ITransaction
{
    public string? ApprenticeName { get; set; }
    public string? ApprenticeshipTrainingCourse { get; set; }
    public int CourseLevel { get; set; }
    public string Description { get; set; }
    public decimal EnglishPercentage { get; set; }
    public decimal GovernmentContribution { get; set; }
    public required Guid Id { get; set; }
    public decimal LevyDeclared { get; set; }
    public decimal PaidFromLevy { get; set; }
    public string? PayeScheme { get; set; }
    public DateTime PayrollMonth { get; set; }
    public decimal TenPercentageTopUp { get; set; }
    public decimal Total { get; set; }
    public required DateTime TransactionDate { get; set; }
    public required string TransactionType { get; set; }
    public string? TrainingProvider { get; set; }
    public decimal? ULN { get; set; }
    public decimal YourContribution { get; set; }
}