using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Repo.Interface
{
	public interface IGenericRepository<T> where T : class
	{
		Task<T?> GetByIdAsync(int id);
		Task<T?> GetByIdAsync(string id);
		Task<T?> GetByIdAsync(short id);
		IQueryable<T> GetQueryable();
		Task<IEnumerable<T>> GetAllAsync();
		IQueryable<T> Find(Expression<Func<T, bool>> predicate);
		Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
		Task<T> AddAsync(T entity);
		Task AddRangeAsync(IEnumerable<T> entities);
		void Update(T entity);
		void UpdateRange(IEnumerable<T> entities);
		void Delete(T entity);
		void DeleteRange(IEnumerable<T> entities);
		Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
		Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
		Task<int> SaveChangesAsync();
	}
}
