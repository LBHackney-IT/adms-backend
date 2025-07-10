using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IAuditLog
{
    JsonDocument Details { get; set; }
    AuditLogEventType EventType { get; set; }
    string EventTypeTargetId { get; set; }
    Guid Id { get; set; }
    AuditLogStatus Status { get; set; }
    DateTime CreatedAt { get; set; }
    string? UserId { get; set; }
}