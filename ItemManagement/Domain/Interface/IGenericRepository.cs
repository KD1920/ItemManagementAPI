using System.Linq.Expressions;

namespace ItemManagement.Domain.Interface;

public interface IGenericRepository<T> where T : class
{
	Task<List<T>> GetAllAsync();
	Task<T> GetByIdAsync(int id);
	Task<T> AddAsync(T entity);
	Task<T> UpdateAsync(T entity);
	Task<T> DeleteAsync(int id);
	Task DeleteRangeAsync(List<T> entity);
	// Task<int> GetCountAsync();
	Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
	Task<List<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);
}
