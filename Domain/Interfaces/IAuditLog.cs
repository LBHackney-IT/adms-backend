using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IAuditLog
{
    JsonDocument Details { get; set; }
    AuditLogEventType EventType { get; set; }
    Guid Id { get; set; }
    AuditLogStatus Status { get; set; }
    //TimestampAttribute Timestamp { get; set; }
    string? UserId { get; set; }
}