using System.ComponentModel;

namespace Domain.Enums;

public enum Classification
{
    [Description("Career Development Qualification (Current Hackney Council internal staff upskilling on apprenticeship)")]
    CareerDevelopmentQualification,

    [Description("Hackney Council employee newly recruited to an apprentice role")]
    NewlyRecruitedHackneyCouncilEmployee,
    
    [Description("New recruited apprentice employed by a Hackney maintained school")]
    NewlyRecruitedSchoolEmployee,

    [Description("Current employee at a Hackney maintained school upskilling")]
    UpskillingSchoolEmployee
}