namespace FiveMinutes.Interfaces;

public interface IDefaultRepository<T>
{
    bool Add(T fmt);

    bool Update(T fmt);

    bool Delete(T fmt);

    bool Save();
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetByIdAsyncNoTracking(int id);
}