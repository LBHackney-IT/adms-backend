using System.Text.Json;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Tests.Interceptors;

public class AuditSaveChangesInterceptorTests
{
    private static ApplicationDbContext CreateContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName)
            .AddInterceptors(new AuditSaveChangesInterceptor())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static Apprentice CreateApprentice(Guid id)
    {
        return new Apprentice
        {
            Id = id,
            Name = "Test Apprentice",
            StartDate = DateTime.UtcNow.Date,
            Status = "Active",
            ULN = 1234567890,
            DateOfBirth = DateTime.UtcNow.Date.AddYears(-20),
            CreatedAt = DateTime.UtcNow
        };
    }

    private static Transaction CreateTransaction(Guid id)
    {
        return new Transaction
        {
            Id = id,
            Description = "Test Transaction",
            TransactionDate = DateTime.UtcNow.Date,
            TransactionType = "Payment",
            CreatedAt = DateTime.UtcNow
        };
    }

    [Fact]
    public void SavingModifiedEntity_CreatesAuditLog()
    {
        using var context = CreateContext(Guid.NewGuid().ToString());
        var apprentice = CreateApprentice(Guid.NewGuid());
        context.Apprentices.Add(apprentice);
        context.SaveChanges();

        apprentice.Name = "Updated Apprentice";
        context.SaveChanges();

        var auditLogs = context.AuditLogs.ToList();
        Assert.Contains(auditLogs, log => log.EventType == AuditLogEventType.ApprenticeUpdated);
    }

    [Fact]
    public void SavingMultipleEntities_CreatesMultipleAuditLogs()
    {
        using var context = CreateContext(Guid.NewGuid().ToString());
        context.Apprentices.Add(CreateApprentice(Guid.NewGuid()));
        context.Transactions.Add(CreateTransaction(Guid.NewGuid()));
        context.SaveChanges();

        var auditLogs = context.AuditLogs.ToList();
        Assert.Equal(2, auditLogs.Count);
        Assert.Contains(auditLogs, log => log.EventType == AuditLogEventType.ApprenticeAdded);
        Assert.Contains(auditLogs, log => log.EventType == AuditLogEventType.TransactionAdded);
    }

    [Fact]
    public void SavingAuditLog_DoesNotCreateRecursion()
    {
        using var context = CreateContext(Guid.NewGuid().ToString());
        context.AuditLogs.Add(new AuditLog
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            EventType = AuditLogEventType.DataIngestion,
            EventTypeTargetId = "seed",
            Status = AuditLogStatus.Success,
            Details = JsonDocument.Parse("{\"operation\":\"Seed\"}"),
            UserId = null
        });

        context.SaveChanges();

        Assert.Single(context.AuditLogs);
    }
}
