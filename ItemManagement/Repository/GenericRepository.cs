using System.Linq.Expressions;
using ItemManagement.Common.Service.Interface;
using ItemManagement.Data;
using ItemManagement.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace ItemManagement.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ItemManagementDbContext _context;
    private readonly DbSet<T> _dbSet;

    private readonly ICommonService _commonService;

    public GenericRepository(
        ItemManagementDbContext context,
        ICommonService commonService
    )
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _commonService = commonService;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var response = await _dbSet.FindAsync(id);
        
        return response;
    }

    public async Task<T> AddAsync(T entity)
    {
        var user = await _commonService.GetUser();

        var createdByProp = typeof(T).GetProperty("CreatedBy");

        if (createdByProp != null && createdByProp.CanWrite)
            createdByProp.SetValue(entity, user.UserId);

        var newEntity = await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return newEntity.Entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        var now = DateTime.UtcNow.ToString();
        var user = await _commonService.GetUser();

        var modifiedOnProp = typeof(T).GetProperty("ModifiedOn");
        var modifiedByProp = typeof(T).GetProperty("ModifiedBy");

        if (modifiedOnProp != null && modifiedOnProp.CanWrite)
            modifiedOnProp.SetValue(entity, DateTime.Parse(now));

        if (modifiedByProp != null && modifiedByProp.CanWrite)
            modifiedByProp.SetValue(entity, user.UserId);

        var newEntity = _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return newEntity.Entity;
    }

    public async Task<T> DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return null;

        var newEntity = _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return newEntity.Entity;
    }

    public async Task DeleteRangeAsync(List<T> entity)
    {
        _dbSet.RemoveRange(entity);
        await _context.SaveChangesAsync();
    }

    // public async Task<int> GetCountAsync()
    // {
    //     return await _dbSet.CountAsync();
    // }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        var isExists = await _dbSet.AnyAsync(predicate);
        return isExists;
    }

    public async Task<List<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }
}
