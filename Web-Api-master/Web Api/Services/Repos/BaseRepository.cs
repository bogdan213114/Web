using Microsoft.EntityFrameworkCore;
using System.Linq;
using Web_Api.Models;

namespace Web_Api.Services;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseModel, new()
{
    protected readonly WebApiContext _db;
    protected readonly DbSet<T> _dbSet;
    public BaseRepository(WebApiContext context)
    {
        _db = context;
        _dbSet = _db.Set<T>();
    }

    public async Task<T> GetByIdAsync(long id)
    {
        T item = await _dbSet.FindAsync(id);
        if (item is null)
        {
            return null;
        }
        return item;
    }
    public IQueryable<T> GetAll()
    {
        return _dbSet;
    }
    public void DeleteById(long id)
    {
        T result = _dbSet.Find(id);
        if (result is not null)
        {
            _dbSet.Remove(result);
        }
    }
    public async Task<long> CreateAsync(T item)
    {
        var result = await _dbSet.AddAsync(item);
        return result.Entity.Id;
    }
    public void Update(T item)
    {
        _dbSet.Update(item);
    }
    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}
