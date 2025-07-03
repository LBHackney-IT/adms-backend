using System;
using System.Linq.Expressions;
using Application.Apprentices;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class ReadApprenticeRepository: IReadRepository<Apprentice, ResponseApprenticeDto>
{
    
    private readonly ApplicationDbContext _context;
    
    public ReadApprenticeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ResponseApprenticeDto>> GetAllAsync()
    {
        return await (from a in _context.Apprentices
            join t in _context.Transactions on a.ULN equals t.ULN into transactions
            select new ResponseApprenticeDto
            {
                Id = a.Id,
                Name = a.Name,
                ULN = a.ULN,
                StartDate = a.StartDate,
                Status = a.Status,
                ApprenticeAchievement = a.ApprenticeAchievement,
                ApprenticeConfirmation = a.ApprenticeConfirmation,
                ApprenticeClassification = a.ApprenticeClassification,
                ApprenticeEthnicity = a.ApprenticeEthnicity,
                ApprenticeGender = a.ApprenticeGender,
                ApprenticeNonCompletionReason = a.ApprenticeNonCompletionReason,
                ApprenticeProgram = a.ApprenticeProgram,
                ApprenticeProgression = a.ApprenticeProgression,
                ApprenticeshipDelivery = a.ApprenticeshipDelivery,
                CertificatesReceived = a.CertificatesReceived,
                CompletionDate = a.CompletionDate,
                DateOfBirth = a.DateOfBirth,
                Directorate = a.Directorate,
                DoeReference = a.DoeReference,
                EmployeeNumber = a.EmployeeNumber,
                EndDate = a.EndDate,
                EndPointAssessor = a.EndPointAssessor,
                IsCareLeaver = a.IsCareLeaver,
                IsDisabled = a.IsDisabled,
                ManagerName = a.ManagerName,
                ManagerTitle = a.ManagerTitle,
                PauseDate = a.PauseDate,
                Post = a.Post,
                School = a.School,
                TotalAgreedApprenticeshipPrice = a.TotalAgreedApprenticeshipPrice,
                TrainingCourse = a.TrainingCourse,
                TrainingProvider = a.TrainingProvider,
                UKPRN = a.UKPRN,
                WithdrawalDate = a.WithdrawalDate,
                // Enriched data from the Transaction entity
                Transactions = transactions.ToList()
            }).ToListAsync();
    }

    public async Task<Apprentice> GetByIdAsync(Guid id)
    {
        var apprenticeById = await _context.Apprentices.FindAsync(id);
        return apprenticeById;
    }

    public async Task<List<ResponseApprenticeDto>> GetByUlnAsync(decimal uln)
    {
        return await (from a in _context.Apprentices.Where(a => a.ULN == uln)
            join t in _context.Transactions on a.ULN equals t.ULN into transactions
            select new ResponseApprenticeDto
            {
                Id = a.Id,
                Name = a.Name,
                ULN = a.ULN,
                StartDate = a.StartDate,
                Status = a.Status,
                ApprenticeAchievement = a.ApprenticeAchievement,
                ApprenticeConfirmation = a.ApprenticeConfirmation,
                ApprenticeClassification = a.ApprenticeClassification,
                ApprenticeEthnicity = a.ApprenticeEthnicity,
                ApprenticeGender = a.ApprenticeGender,
                ApprenticeNonCompletionReason = a.ApprenticeNonCompletionReason,
                ApprenticeProgram = a.ApprenticeProgram,
                ApprenticeProgression = a.ApprenticeProgression,
                ApprenticeshipDelivery = a.ApprenticeshipDelivery,
                CertificatesReceived = a.CertificatesReceived,
                CompletionDate = a.CompletionDate,
                DateOfBirth = a.DateOfBirth,
                Directorate = a.Directorate,
                DoeReference = a.DoeReference,
                EmployeeNumber = a.EmployeeNumber,
                EndDate = a.EndDate,
                EndPointAssessor = a.EndPointAssessor,
                IsCareLeaver = a.IsCareLeaver,
                IsDisabled = a.IsDisabled,
                ManagerName = a.ManagerName,
                ManagerTitle = a.ManagerTitle,
                PauseDate = a.PauseDate,
                Post = a.Post,
                School = a.School,
                TotalAgreedApprenticeshipPrice = a.TotalAgreedApprenticeshipPrice,
                TrainingCourse = a.TrainingCourse,
                TrainingProvider = a.TrainingProvider,
                UKPRN = a.UKPRN,
                WithdrawalDate = a.WithdrawalDate,
                // Enriched data from the Transaction entity
                Transactions = transactions.ToList()
            }).ToListAsync();
    }

    public async Task<List<ResponseApprenticeDto>> FindAsync(Expression<Func<Apprentice, bool>> predicate)
    {
        return await (from a in _context.Apprentices.Where(predicate)
            join t in _context.Transactions on a.ULN equals t.ULN into apprenticeTransactions
            select new ResponseApprenticeDto
            {
                Id = a.Id,
                Name = a.Name,
                ULN = a.ULN,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                Status = a.Status,
                ApprenticeAchievement = a.ApprenticeAchievement,
                ApprenticeConfirmation = a.ApprenticeConfirmation,
                ApprenticeClassification = a.ApprenticeClassification,
                ApprenticeEthnicity = a.ApprenticeEthnicity,
                ApprenticeGender = a.ApprenticeGender,
                ApprenticeNonCompletionReason = a.ApprenticeNonCompletionReason,
                ApprenticeProgram = a.ApprenticeProgram,
                ApprenticeProgression = a.ApprenticeProgression,
                ApprenticeshipDelivery = a.ApprenticeshipDelivery,
                CertificatesReceived = a.CertificatesReceived,
                CompletionDate = a.CompletionDate,
                DateOfBirth = a.DateOfBirth,
                Directorate = a.Directorate,
                DoeReference = a.DoeReference,
                EmployeeNumber = a.EmployeeNumber,
                EndPointAssessor = a.EndPointAssessor,
                IsCareLeaver = a.IsCareLeaver,
                IsDisabled = a.IsDisabled,
                ManagerName = a.ManagerName,
                ManagerTitle = a.ManagerTitle,
                PauseDate = a.PauseDate,
                Post = a.Post,
                School = a.School,
                TotalAgreedApprenticeshipPrice = a.TotalAgreedApprenticeshipPrice,
                TrainingCourse = a.TrainingCourse,
                TrainingProvider = a.TrainingProvider,
                UKPRN = a.UKPRN,
                WithdrawalDate = a.WithdrawalDate,
                // Enriched data from the Transaction entity
                Transactions = apprenticeTransactions.ToList()
            }).ToListAsync();
    }
}