using Domain.Enums;

namespace Application.Apprentices;

public class FindApprentice
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; }
    public DirectorateCode? Directorate { get; set; }
    public ApprenticeshipProgram? ApprenticeProgram { get; set; }
}