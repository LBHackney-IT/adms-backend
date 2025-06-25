using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
        };
        await _context.Transactions.AddAsync(newTransaction);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<WriteTransactionDto> entities)
    {
        var transactionList = entities.ToList();
        
        var transactions = transactionList.Select(dtolistitem => new Transaction
        {
            Id = Guid.NewGuid(),
            ApprenticeName = dtolistitem.ApprenticeName,
            ApprenticeshipTrainingCourse = dtolistitem.ApprenticeshipTrainingCourse,
            CourseLevel = dtolistitem.CourseLevel,
            Description = dtolistitem.Description,
            EnglishPercentage = dtolistitem.EnglishPercentage,
            GovernmentContribution = dtolistitem.GovernmentContribution,
            LevyDeclared = dtolistitem.LevyDeclared,
            PaidFromLevy = dtolistitem.PaidFromLevy,
            PayeScheme = dtolistitem.PayeScheme,
            PayrollMonth = dtolistitem.PayrollMonth,
            
            TenPercentageTopUp = dtolistitem.TenPercentageTopUp,
            Total = dtolistitem.Total,
            TransactionDate = dtolistitem.TransactionDate,
            TransactionType = dtolistitem.TransactionType,
            TrainingProvider = dtolistitem.TrainingProvider,
            ULN = dtolistitem.ULN,
            YourContribution = dtolistitem.YourContribution,
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