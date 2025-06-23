using System;
using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs;

public class ResponseTransactionDto
{
    
    public Guid Id { get; set; }
    public string? ApprenticeName { get; set; }
    public string? ApprenticeshipTrainingCourse { get; set; }
    public int CourseLevel { get; set; }
    public string Description { get; set; }
    public decimal EnglishPercentage { get; set; }
    public decimal GovernmentContribution { get; set; }
    public decimal LevyDeclared { get; set; }
    public decimal PaidFromLevy { get; set; }
    public string? PayeScheme { get; set; }
    public DateTime PayrollMonth { get; set; }
    public decimal TenPercentageTopUp { get; set; }
    public decimal Total { get; set; }
    public DateTime TransactionDate { get; set; }
    public string TransactionType { get; set; }
    public string? TrainingProvider { get; set; }
    public decimal? ULN { get; set; }
    public decimal YourContribution { get; set; }

    // Enriched data from the Apprentice entity
    public DirectorateCode? ApprenticeDirectorate { get; set; }
    public ApprenticeshipProgram? ApprenticeProgram { get; set; }
    public string? ApprenticeStatus { get; set; }
    
}