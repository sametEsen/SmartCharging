using System.Linq.Expressions;

namespace SmartCharging.Domain.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		Task<T?> GetByIdAsync(params object[] keyValues);
		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
		Task AddAsync(T entity);
		void Delete(T entity);
		void Update(T entity);
	}
}
