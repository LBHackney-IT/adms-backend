using System.Text.Json;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Data.Interceptors;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        if (context != null)
        {
            AddAuditLogs(context);
        }

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context != null)
        {
            AddAuditLogs(context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void AddAuditLogs(DbContext context)
    {
        var auditLogs = new List<AuditLog>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog)
            {
                continue;
            }

            if (entry.State is EntityState.Detached or EntityState.Unchanged)
            {
                continue;
            }

            if (entry.Entity is not Apprentice && entry.Entity is not Transaction)
            {
                continue;
            }

            var auditLog = CreateAuditLog(entry);
            if (auditLog != null)
            {
                auditLogs.Add(auditLog);
            }
        }

        if (auditLogs.Count > 0)
        {
            context.Set<AuditLog>().AddRange(auditLogs);
        }
    }

    private static AuditLog? CreateAuditLog(EntityEntry entry)
    {
        var eventType = GetEventType(entry);
        if (eventType == null)
        {
            return null;
        }

        return new AuditLog
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            EventType = eventType.Value,
            EntityName = entry.Entity.GetType().Name,
            EventTypeTargetId = GetPrimaryKeyString(entry),
            Status = AuditLogStatus.Success,
            Details = BuildDetails(entry),
            UserId = null,
            CorrelationId = null
        };
    }

    private static AuditLogEventType? GetEventType(EntityEntry entry)
    {
        return entry.Entity switch
        {
            Apprentice when entry.State == EntityState.Added => AuditLogEventType.ApprenticeAdded,
            Apprentice when entry.State == EntityState.Modified => AuditLogEventType.ApprenticeUpdated,
            Apprentice when entry.State == EntityState.Deleted => AuditLogEventType.ApprenticeDeleted,
            Transaction when entry.State == EntityState.Added => AuditLogEventType.TransactionAdded,
            Transaction when entry.State == EntityState.Modified => AuditLogEventType.TransactionUpdated,
            Transaction when entry.State == EntityState.Deleted => AuditLogEventType.TransactionDeleted,
            _ => null
        };
    }

    private static JsonDocument BuildDetails(EntityEntry entry)
    {
        var entityName = entry.Entity.GetType().Name;
        var key = GetPrimaryKeyString(entry);

        return entry.State switch
        {
            EntityState.Added => JsonSerializer.SerializeToDocument(new
            {
                entity = entityName,
                operation = "Added",
                key,
                values = GetCurrentValues(entry)
            }, JsonOptions),
            EntityState.Deleted => JsonSerializer.SerializeToDocument(new
            {
                entity = entityName,
                operation = "Deleted",
                key,
                values = GetOriginalValues(entry)
            }, JsonOptions),
            EntityState.Modified => JsonSerializer.SerializeToDocument(new
            {
                entity = entityName,
                operation = "Modified",
                key,
                changes = GetModifiedValues(entry)
            }, JsonOptions),
            _ => JsonSerializer.SerializeToDocument(new { entity = entityName, operation = entry.State.ToString(), key }, JsonOptions)
        };
    }

    private static Dictionary<string, object?> GetCurrentValues(EntityEntry entry)
    {
        return entry.Properties.ToDictionary(
            property => property.Metadata.Name,
            property => property.CurrentValue);
    }

    private static Dictionary<string, object?> GetOriginalValues(EntityEntry entry)
    {
        return entry.Properties.ToDictionary(
            property => property.Metadata.Name,
            property => property.OriginalValue);
    }

    private static Dictionary<string, object?> GetModifiedValues(EntityEntry entry)
    {
        return entry.Properties
            .Where(property => property.IsModified)
            .ToDictionary(
                property => property.Metadata.Name,
                property => (object?)new { original = property.OriginalValue, current = property.CurrentValue });
    }

    private static string GetPrimaryKeyString(EntityEntry entry)
    {
        var key = entry.Metadata.FindPrimaryKey();
        if (key == null)
        {
            return string.Empty;
        }

        var keyValues = key.Properties
            .Select(p => entry.Property(p.Name).CurrentValue ?? entry.Property(p.Name).OriginalValue)
            .Where(value => value != null)
            .Select(value => value!.ToString());

        return string.Join(",", keyValues);
    }
}
