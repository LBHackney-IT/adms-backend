using System;
using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IReadRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetByUlnAsync(decimal uln);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}