using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories;

public class ReadTransactionRepository : IReadRepository<Transaction>
{

    private readonly ApplicationDbContext _context;

    public ReadTransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await _context.Transactions.ToListAsync();
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

    public async Task<IEnumerable<Transaction>> FindAsync(Expression<Func<Transaction, bool>> predicate)
    {
        return await _context.Transactions.Where(predicate).ToListAsync();
    }
}