using System;
using System.Xml;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Apprentice> Apprentices { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //Transaction Entity
        modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
        modelBuilder.Entity<Transaction>().Property(t => t.Description).IsRequired();
        modelBuilder.Entity<Transaction>().Property(t => t.TransactionDate).IsRequired();
        //Apprentice Entity
        modelBuilder.Entity<Apprentice>().HasKey(a => a.Id);
        modelBuilder.Entity<Apprentice>().HasIndex(a => a.ULN).IsUnique();
        modelBuilder.Entity<Apprentice>().Property(a => a.ULN).IsRequired();
        modelBuilder.Entity<Apprentice>().Property(a => a.StartDate).IsRequired();
        modelBuilder.Entity<Apprentice>().Property(a => a.ApprenticeAchievement).HasConversion<string>();
        modelBuilder.Entity<Apprentice>().Property(a => a.ApprenticeClassification).HasConversion<string>();
        modelBuilder.Entity<Apprentice>().Property(a => a.ApprenticeEthnicity).HasConversion<string>();
        modelBuilder.Entity<Apprentice>().Property(a => a.ApprenticeGender).HasConversion<string>();
        modelBuilder.Entity<Apprentice>().Property(a => a.ApprenticeNonCompletionReason).HasConversion<string>();
        modelBuilder.Entity<Apprentice>().Property(a => a.ApprenticeProgram).HasConversion<string>();
        modelBuilder.Entity<Apprentice>().Property(a => a.ApprenticeProgression).HasConversion<string>();
        modelBuilder.Entity<Apprentice>().Property(a => a.CertificatesReceived).HasConversion<string>();
        modelBuilder.Entity<Apprentice>().Property(a => a.Directorate).HasConversion<string>();
        //AuditLog Entity
        modelBuilder.Entity<AuditLog>().HasKey(a => a.Id);
        modelBuilder.Entity<AuditLog>().Property(a => a.EventType).HasConversion<string>();
        modelBuilder.Entity<AuditLog>().Property(a => a.Status).HasConversion<string>();
    }
}