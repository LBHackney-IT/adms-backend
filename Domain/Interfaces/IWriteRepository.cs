using System;

namespace Domain.Interfaces;

public interface IWriteRepository<TEntity, TGenerate> where TEntity : class where TGenerate : class
{
    Task AddAsync(TGenerate entity);
    Task AddRangeAsync(IEnumerable<TGenerate> entities);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(Guid id);
}