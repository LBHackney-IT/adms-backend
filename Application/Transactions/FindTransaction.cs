namespace Application.DTOs;

public class FindTransaction
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Description { get; set; }
}