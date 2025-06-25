using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Domain.Interfaces;
using Domain.Enums;

namespace Domain.Entities;

public class AuditLog: IAuditLog
{
    public JsonDocument Details { get; set; }
    public AuditLogEventType EventType { get; set; }
    public Guid Id { get; set; }
    public AuditLogStatus Status { get; set; }
    //public TimestampAttribute Timestamp { get; set; }
    public string? UserId { get; set; }
}