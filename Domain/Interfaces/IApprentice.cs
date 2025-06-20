using Domain.Enums;

namespace Domain.Interfaces;

public interface IApprentice
{
    Achievement ApprenticeAchievement { get; set; }
    string? ApprenticeConfirmation { get; set; }
    Classification ApprenticeClassification { get; set; }
    Ethnicity ApprenticeEthnicity { get; set; }
    Gender ApprenticeGender { get; set; }
    NonCompletionReason? ApprenticeNonCompletionReason { get; set; }
    ApprenticeshipProgram ApprenticeProgram { get; set; }
    ProgressionTracker ApprenticeProgression { get; set; }
    string? ApprenticeshipDelivery { get; set; }
    CertificateStatus CertificatesReceived { get; set; }
    DateTime CompletionDate { get; set; }
    DateTime DateOfBirth { get; set; }
    DirectorateCode Directorate { get; set; }
    string? DoeReference { get; set; }
    string? EmployeeNumber { get; set; }
    DateTime EndDate { get; set; }
    string? EndPointAssessor { get; set; }
    Guid Id { get; set; }
    bool IsCareLeaver { get; set; }
    bool IsDisabled { get; set; }
    string? ManagerName { get; set; }
    string? ManagerTitle { get; set; }
    string Name { get; set; }
    DateTime PauseDate { get; set; }
    string? Post { get; set; }
    string? School { get; set; }
    DateTime StartDate { get; set; }
    string Status { get; set; }
    decimal TotalAgreedApprenticeshipPrice { get; set; }
    string? TrainingCourse { get; set; }
    string? TrainingProvider { get; set; }
    decimal UKPRN { get; set; }
    decimal ULN { get; set; }
    DateTime? WithdrawalDate { get; set; }
}