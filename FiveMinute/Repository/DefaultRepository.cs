using FiveMinute.Data;
using FiveMinute.Interfaces;

namespace FiveMinute.Repository;

public abstract class DefaultRepository<T> : IDefaultRepository<T>
{
    protected readonly ApplicationDbContext context;

    public DefaultRepository(ApplicationDbContext context)
    {
        this.context = context;
    }
    public async Task<bool> Add(T obj)
    {
        context.Add(obj);
        return await Save();
    }

    public async Task<bool> Delete(T obj)
    {
        context.Remove(obj);
        return await Save();
    }
    public async Task<bool> Save()
    {
        var saved = context.SaveChanges();
        return saved >= 0 ? true : false;
    }

}
