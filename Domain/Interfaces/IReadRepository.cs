using System;
using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IReadRepository<TEntity, TResponse> 
    where TEntity : class 
    where TResponse : class
{
    Task<List<TResponse>> GetAllAsync();
    Task<TEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetByUlnAsync(decimal uln);
    Task<List<TResponse>> FindAsync(Expression<Func<TEntity, bool>> predicate);
}
