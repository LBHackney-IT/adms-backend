using System.Text.Json;
using Domain.Enums;

namespace Application.AuditLogs;

public class ResponseAuditLogDto
{
    public required Guid Id { get; set; }
    public required AuditLogEventType EventType { get; set; }
    public required AuditLogStatus Status { get; set; }
    public required string EventTypeTargetId { get; set; }
    public required JsonDocument Details { get; set; }
    public string? UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
