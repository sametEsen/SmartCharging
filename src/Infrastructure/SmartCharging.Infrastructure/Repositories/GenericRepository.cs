using Microsoft.EntityFrameworkCore;
using SmartCharging.Domain.Interfaces;
using SmartCharging.Infrastructure.Contexts;
using System.Linq.Expressions;

namespace SmartCharging.Infrastructure.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected readonly SmartChargingContext _dbContext;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(SmartChargingContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}

		public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

		public void Delete(T entity) => _dbSet.Remove(entity);

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
		{
			IQueryable<T> query = _dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query.ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public void Update(T entity) => _dbSet.Update(entity);
	}
}
