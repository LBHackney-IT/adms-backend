using System;
using System.Text.Json;
using Domain.Interfaces;
using Domain.Enums;

namespace Domain.Entities;

public class AuditLog: IAuditLog
{
    public JsonDocument Details { get; set; }
    public AuditLogEventType EventType { get; set; }
    //this will store the apprentice/transaction Id as a string for easier data management
    public string EventTypeTargetId { get; set; }
    public Guid Id { get; set; }
    public AuditLogStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UserId { get; set; }
}