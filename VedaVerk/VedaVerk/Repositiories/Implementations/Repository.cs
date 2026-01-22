using Microsoft.EntityFrameworkCore;
using VedaVerk.Data;
using VedaVerk.Models;
using VedaVerk.Repositiories.Interfaces;

namespace VedaVerk.Repositiories.Implementations
{
	public class Repository<T> : IRepository<T> where T : class
	{
		public readonly ApplicationDbContext _context;
		public readonly DbSet<T> _dbSet;

		public Repository(ApplicationDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}

		public async Task AddAsync(T entity)
		{
			ArgumentNullException.ThrowIfNull(entity);

			await _dbSet.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			if (id <= 0)
				throw new ArgumentException("Invalid ID", nameof(id));

			var entity = _dbSet.Find(id) ?? throw new InvalidOperationException("Entity not found");

			_dbSet.Remove(entity);

			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public async Task<T?> GetByIdAsync(int id)
		{
			if (id <= 0)
				throw new ArgumentException("Invalid ID", nameof(id));

			return await _dbSet.FindAsync(id);
		}

		public async Task UpdateAsync(T entity)
		{
			ArgumentNullException.ThrowIfNull(entity);

			_dbSet.Update(entity);

			await _context.SaveChangesAsync();
		}
	}
}
