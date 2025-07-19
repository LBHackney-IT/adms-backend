
using Domain.Entities;
using Domain.Enums;

namespace Application.Apprentices;

public class ResponseApprenticeDto
{
    public Achievement? ApprenticeAchievement { get; set; }
    public string? ApprenticeConfirmation { get; set; }
    public Classification? ApprenticeClassification { get; set; }
    public Ethnicity? ApprenticeEthnicity { get; set; }
    public Gender? ApprenticeGender { get; set; }
    public NonCompletionReason? ApprenticeNonCompletionReason { get; set; }
    public ApprenticeshipProgram? ApprenticeProgram { get; set; }
    public ProgressionTracker? ApprenticeProgression { get; set; }
    public string? ApprenticeshipDelivery { get; set; }
    public CertificateStatus? CertificatesReceived { get; set; }
    public DateTime? CompletionDate { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public DirectorateCode? Directorate { get; set; }
    public string? DoeReference { get; set; }
    public string? EmployeeNumber { get; set; }
    public DateTime? EndDate { get; set; }
    public string? EndPointAssessor { get; set; }
    public required Guid Id { get; set; }
    public bool IsCareLeaver { get; set; } = false;
    public bool IsDisabled { get; set; } = false;
    public string? ManagerName { get; set; }
    public string? ManagerTitle { get; set; }
    public required string Name { get; set; }
    public DateTime? PauseDate { get; set; }
    public string? Post { get; set; }
    public string? School { get; set; }
    public required DateTime StartDate { get; set; }
    public required string Status { get; set; }
    public decimal TotalAgreedApprenticeshipPrice { get; set; }
    public string? TrainingCourse { get; set; }
    public string? TrainingProvider { get; set; }
    public decimal? UKPRN { get; set; }
    public required decimal ULN { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Enriched data from Transactions
    public List<Transaction> Transactions { get; set; } = new();
    
}