using System;

namespace Domain.Interfaces;

public interface IWriteRepository<TEntity, TGenerate> where TEntity : class where TGenerate : class
{
    Task AddAsync(TGenerate dto);
    Task UploadAsync(IEnumerable<TGenerate> dtos);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(Guid id);
}