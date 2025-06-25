using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Application.DTOs;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class ReadTransactionRepository : IReadRepository<Transaction, ResponseTransactionDto>
{

    private readonly ApplicationDbContext _context;

    public ReadTransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    
    public async Task<List<ResponseTransactionDto>> GetAllAsync()
    {
        return await (from t in _context.Transactions
            join a in _context.Apprentices on t.ULN equals a.ULN into apprentices
            from apprentice in apprentices.DefaultIfEmpty()
            select new ResponseTransactionDto
            {
                Id = t.Id,
                ApprenticeName = t.ApprenticeName,
                ApprenticeshipTrainingCourse = t.ApprenticeshipTrainingCourse,
                CourseLevel = t.CourseLevel,
                Description = t.Description,
                EnglishPercentage = t.EnglishPercentage,
                GovernmentContribution = t.GovernmentContribution,
                LevyDeclared = t.LevyDeclared,
                PaidFromLevy = t.PaidFromLevy,
                PayeScheme = t.PayeScheme,
                PayrollMonth = t.PayrollMonth,
                TenPercentageTopUp = t.TenPercentageTopUp,
                Total = t.Total,
                TransactionDate = t.TransactionDate,
                TransactionType = t.TransactionType,
                TrainingProvider = t.TrainingProvider,
                ULN = t.ULN,
                YourContribution = t.YourContribution,
                // Enriched data from the Apprentice entity
                ApprenticeDirectorate = apprentice != null ? apprentice.Directorate : null,
                ApprenticeProgram = apprentice != null ? apprentice.ApprenticeProgram : null,
                ApprenticeStatus = apprentice != null ? apprentice.Status : null
            }).ToListAsync();
    }

    public async Task<Transaction> GetByIdAsync(Guid id)
    {
        var transactionById = await _context.Transactions.FindAsync(id);
        return transactionById;
    }

    public async Task<IEnumerable<Transaction>> GetByUlnAsync(decimal uln)
    {
        var transactionsByULN = await _context.Transactions.Where(t => t.ULN == uln).ToListAsync();
        return transactionsByULN;
    }

    public async Task<List<ResponseTransactionDto>> FindAsync(Expression<Func<Transaction, bool>> predicate)
    {
        return await (from t in _context.Transactions.Where(predicate)
            join a in _context.Apprentices on t.ULN equals a.ULN into apprentices
            from apprentice in apprentices.DefaultIfEmpty()
            select new ResponseTransactionDto
            {
                Id = t.Id,
                ApprenticeName = t.ApprenticeName,
                ApprenticeshipTrainingCourse = t.ApprenticeshipTrainingCourse,
                CourseLevel = t.CourseLevel,
                Description = t.Description,
                EnglishPercentage = t.EnglishPercentage,
                GovernmentContribution = t.GovernmentContribution,
                LevyDeclared = t.LevyDeclared,
                PaidFromLevy = t.PaidFromLevy,
                PayeScheme = t.PayeScheme,
                PayrollMonth = t.PayrollMonth,
                TenPercentageTopUp = t.TenPercentageTopUp,
                Total = t.Total,
                TransactionDate = t.TransactionDate,
                TransactionType = t.TransactionType,
                TrainingProvider = t.TrainingProvider,
                ULN = t.ULN,
                YourContribution = t.YourContribution,
                ApprenticeDirectorate = apprentice != null ? apprentice.Directorate : null,
                ApprenticeProgram = apprentice != null ? apprentice.ApprenticeProgram : null,
                ApprenticeStatus = apprentice != null ? apprentice.Status : null
            }).ToListAsync();
    }
}