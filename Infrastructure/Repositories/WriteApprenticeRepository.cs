using System;
using Application.Apprentices;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


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
            CreatedAt = DateTime.UtcNow
        };
        await _context.Apprentices.AddAsync(newApprentice);
        await _context.SaveChangesAsync();
    }

    public async Task UploadAsync(IEnumerable<WriteApprenticeDto> entities)
    {
        // incoming data into List
        var apprenticeDtos = entities.ToList();
        
        // extracts all unique ULN from the apprenticeDtos. //recommended step by AI
        var ulns = apprenticeDtos.Select(e => e.ULN).Distinct();
        
        // single database query to fetch all apprentices with matching ulns to the ulns[]. 
        // the result is converted into a `Dictionary`, where the key is the ULN and the value is the Apprentice.
        var existingApprentices = await _context.Apprentices
            .Where(a => ulns.Contains(a.ULN))
            .ToDictionaryAsync(a => a.ULN);
        
        // 
        var apprenticesToAdd = new List<Apprentice>();

        //loop through all elements in the apprenticeDtos[].
        foreach (var entity in apprenticeDtos)
        {
            // check if the ULN is already in the dictionary, then
            if (existingApprentices.TryGetValue(entity.ULN, out var existingApprentice))
            {
                // Update existing apprentices
                // because "var existingApprentices = await _context.Apprentices", `_context` is now "tracking" these entities.
                // It keeps a snapshot of their original state and monitors them for any changes.
                existingApprentice.ApprenticeAchievement = entity.ApprenticeAchievement;
                existingApprentice.ApprenticeConfirmation = entity.ApprenticeConfirmation;
                existingApprentice.ApprenticeClassification = entity.ApprenticeClassification;
                existingApprentice.ApprenticeEthnicity = entity.ApprenticeEthnicity;
                existingApprentice.ApprenticeGender = entity.ApprenticeGender;
                existingApprentice.ApprenticeNonCompletionReason = entity.ApprenticeNonCompletionReason;
                existingApprentice.ApprenticeProgram = entity.ApprenticeProgram;
                existingApprentice.ApprenticeProgression = entity.ApprenticeProgression;
                existingApprentice.ApprenticeshipDelivery = entity.ApprenticeshipDelivery;
                existingApprentice.CertificatesReceived = entity.CertificatesReceived;
                existingApprentice.CompletionDate = entity.CompletionDate;
                existingApprentice.DateOfBirth = entity.DateOfBirth;
                existingApprentice.Directorate = entity.Directorate;
                existingApprentice.DoeReference = entity.DoeReference;
                existingApprentice.EmployeeNumber = entity.EmployeeNumber;
                existingApprentice.EndDate = entity.EndDate;
                existingApprentice.EndPointAssessor = entity.EndPointAssessor;
                existingApprentice.IsCareLeaver = entity.IsCareLeaver;
                existingApprentice.IsDisabled = entity.IsDisabled;
                existingApprentice.ManagerName = entity.ManagerName;
                existingApprentice.ManagerTitle = entity.ManagerTitle;
                existingApprentice.Name = entity.Name;
                existingApprentice.PauseDate = entity.PauseDate;
                existingApprentice.Post = entity.Post;
                existingApprentice.School = entity.School;
                existingApprentice.StartDate = entity.StartDate;
                existingApprentice.Status = entity.Status;
                existingApprentice.TotalAgreedApprenticeshipPrice = entity.TotalAgreedApprenticeshipPrice;
                existingApprentice.TrainingCourse = entity.TrainingCourse;
                existingApprentice.TrainingProvider = entity.TrainingProvider;
                existingApprentice.UKPRN = entity.UKPRN;
                existingApprentice.WithdrawalDate = entity.WithdrawalDate;
            }
            else
            {
                // Add new apprentice to list for bulk insertion
                apprenticesToAdd.Add(new Apprentice
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
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
        
        // checks if apprenticesToAdd contains any elements and adds them to the database
        if (apprenticesToAdd.Any())
        {
            await _context.Apprentices.AddRangeAsync(apprenticesToAdd);
        }
        
        // 
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