using System.Linq;
using Web_Api.Models;


namespace Web_Api.Services;

public interface IRepository<T> where T : BaseModel
{
    Task<T> GetByIdAsync(long id);

    IQueryable<T> GetAll();
    void DeleteById(long id);

    Task<long> CreateAsync(T item);

    void Update(T item);
    Task SaveAsync();
}
