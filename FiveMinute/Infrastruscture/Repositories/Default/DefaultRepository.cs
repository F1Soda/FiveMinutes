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
    public async Task<ResultOperationInDatabase> Add(T obj)
    {
        context.Add(obj);
        return await Save();
    }

    public virtual async Task<ResultOperationInDatabase> Delete(T obj)
    {
        context.Remove(obj);
        return await Save();
    }
    public async Task<ResultOperationInDatabase> Save()
    {
        try {
            var saved = await context.SaveChangesAsync();
            return new ResultOperationInDatabase(saved >= 0, null);
        }
        catch (Exception e) {
            return new ResultOperationInDatabase(false, e);
        }
    }

}
