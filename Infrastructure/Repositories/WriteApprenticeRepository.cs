using System;
using Application.Apprentices;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;


namespace Infrastructure.Repositories;

public class WriteApprenticeRepository: IWriteRepository<Apprentice, WriteApprenticeDto>
{
    
    private readonly ApplicationDbContext _context;
    
    public WriteApprenticeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(WriteApprenticeDto entity)
    {
        var newApprentice = new Apprentice
        {
            ApprenticeAchievement = entity.ApprenticeAchievement,
            ApprenticeConfirmation = entity.ApprenticeConfirmation,
            ApprenticeClassification = entity.ApprenticeClassification,
            ApprenticeEthnicity = entity.ApprenticeEthnicity,
            ApprenticeGender = entity.ApprenticeGender,
            ApprenticeNonCompletionReason = entity.ApprenticeNonCompletionReason,
            ApprenticeProgram = entity.ApprenticeProgram,
            ApprenticeProgression = entity.ApprenticeProgression,
            ApprenticeshipDelivery = entity.ApprenticeshipDelivery,
            CertificatesReceived = entity.CertificatesReceived,
            CompletionDate = entity.CompletionDate,
            DateOfBirth = entity.DateOfBirth,
            Directorate = entity.Directorate,
            DoeReference = entity.DoeReference,
            EmployeeNumber = entity.EmployeeNumber,
            EndDate = entity.EndDate,
            EndPointAssessor = entity.EndPointAssessor,
            Id = Guid.NewGuid(),
            IsCareLeaver = entity.IsCareLeaver,
            IsDisabled = entity.IsDisabled,
            ManagerName = entity.ManagerName,
            ManagerTitle = entity.ManagerTitle,
            Name = entity.Name,
            PauseDate = entity.PauseDate,
            Post = entity.Post,
            School = entity.School,
            StartDate = entity.StartDate,
            Status = entity.Status,
            TotalAgreedApprenticeshipPrice = entity.TotalAgreedApprenticeshipPrice,
            TrainingCourse = entity.TrainingCourse,
            TrainingProvider = entity.TrainingProvider,
            UKPRN = entity.UKPRN,
            ULN = entity.ULN,
            WithdrawalDate = entity.WithdrawalDate,
        };
        await _context.Apprentices.AddAsync(newApprentice);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<WriteApprenticeDto> entities)
    {
        var apprenticeList = entities.ToList();

        var apprentices = apprenticeList.Select(listItem => new Apprentice
        {
            ApprenticeAchievement = listItem.ApprenticeAchievement,
            ApprenticeConfirmation = listItem.ApprenticeConfirmation,
            ApprenticeClassification = listItem.ApprenticeClassification,
            ApprenticeEthnicity = listItem.ApprenticeEthnicity,
            ApprenticeGender = listItem.ApprenticeGender,
            ApprenticeNonCompletionReason = listItem.ApprenticeNonCompletionReason,
            ApprenticeProgram = listItem.ApprenticeProgram,
            ApprenticeProgression = listItem.ApprenticeProgression,
            ApprenticeshipDelivery = listItem.ApprenticeshipDelivery,
            CertificatesReceived = listItem.CertificatesReceived,
            CompletionDate = listItem.CompletionDate,
            DateOfBirth = listItem.DateOfBirth,
            Directorate = listItem.Directorate,
            DoeReference = listItem.DoeReference,
            EmployeeNumber = listItem.EmployeeNumber,
            EndDate = listItem.EndDate,
            EndPointAssessor = listItem.EndPointAssessor,
            Id = Guid.NewGuid(),
            IsCareLeaver = listItem.IsCareLeaver,
            IsDisabled = listItem.IsDisabled,
            ManagerName = listItem.ManagerName,
            ManagerTitle = listItem.ManagerTitle,
            Name = listItem.Name,
            PauseDate = listItem.PauseDate,
            Post = listItem.Post,
            School = listItem.School,
            StartDate = listItem.StartDate,
            Status = listItem.Status,
            TotalAgreedApprenticeshipPrice = listItem.TotalAgreedApprenticeshipPrice,
            TrainingCourse = listItem.TrainingCourse,
            TrainingProvider = listItem.TrainingProvider,
            UKPRN = listItem.UKPRN,
            ULN = listItem.ULN,
            WithdrawalDate = listItem.WithdrawalDate,
        });

        await _context.Apprentices.AddRangeAsync(apprentices);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Apprentice entity)
    {
        var apprentice = _context.Apprentices.Find(entity.Id);
        if(apprentice != null)
            _context.Entry(apprentice).CurrentValues.SetValues(entity);
        await  _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var apprentice = _context.Apprentices.Find(id);
        if(apprentice != null)
            _context.Apprentices.Remove(apprentice);
        await _context.SaveChangesAsync();
    }
}