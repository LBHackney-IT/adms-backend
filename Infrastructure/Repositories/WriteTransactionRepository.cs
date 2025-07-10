using System;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Application.DTOs;


namespace Infrastructure.Repositories;

public class WriteTransactionRepository: IWriteRepository<Transaction, WriteTransactionDto>
{
    
    private readonly ApplicationDbContext _context;

    public WriteTransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(WriteTransactionDto entity)
    {
        var newTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            ApprenticeName = entity.ApprenticeName,
            ApprenticeshipTrainingCourse = entity.ApprenticeshipTrainingCourse,
            CourseLevel = entity.CourseLevel,
            Description = entity.Description,
            EnglishPercentage = entity.EnglishPercentage,
            GovernmentContribution = entity.GovernmentContribution,
            LevyDeclared = entity.LevyDeclared,
            PaidFromLevy = entity.PaidFromLevy,
            PayeScheme = entity.PayeScheme,
            PayrollMonth = entity.PayrollMonth,
            TenPercentageTopUp = entity.TenPercentageTopUp,
            Total = entity.Total,
            TransactionDate = entity.TransactionDate,
            TransactionType = entity.TransactionType,
            TrainingProvider = entity.TrainingProvider,
            ULN = entity.ULN,
            YourContribution = entity.YourContribution,
            CreatedAt = DateTime.UtcNow
        };
        await _context.Transactions.AddAsync(newTransaction);
        
        await _context.SaveChangesAsync();
    }

    public async Task UploadAsync(IEnumerable<WriteTransactionDto> entities)
    {
        var transactionList = entities.ToList();
        
        var transactions = transactionList.Select(listitem => new Transaction
        {
            Id = Guid.NewGuid(),
            ApprenticeName = listitem.ApprenticeName,
            ApprenticeshipTrainingCourse = listitem.ApprenticeshipTrainingCourse,
            CourseLevel = listitem.CourseLevel,
            Description = listitem.Description,
            EnglishPercentage = listitem.EnglishPercentage,
            GovernmentContribution = listitem.GovernmentContribution,
            LevyDeclared = listitem.LevyDeclared,
            PaidFromLevy = listitem.PaidFromLevy,
            PayeScheme = listitem.PayeScheme,
            PayrollMonth = listitem.PayrollMonth,
            TenPercentageTopUp = listitem.TenPercentageTopUp,
            Total = listitem.Total,
            TransactionDate = listitem.TransactionDate,
            TransactionType = listitem.TransactionType,
            TrainingProvider = listitem.TrainingProvider,
            ULN = listitem.ULN,
            YourContribution = listitem.YourContribution,
            CreatedAt = DateTime.UtcNow
        });

        await _context.Transactions.AddRangeAsync(transactions);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Transaction entity)
    {
        var transaction = _context.Transactions.Find(entity.Id);
        if(transaction != null)
            _context.Entry(transaction).CurrentValues.SetValues(entity);
        await  _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var transaction = _context.Transactions.Find(id);
        if(transaction != null)
            _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }
}