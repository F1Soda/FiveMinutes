using FiveMinutes.Interfaces;
using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using System.Diagnostics;

namespace FiveMinutes.Repository;

public abstract class DefaultRepository<T> : IDefaultRepository<T>
{
    protected readonly ApplicationDbContext context;

    public DefaultRepository(ApplicationDbContext context)
    {
        this.context = context;
    }
    public bool Add(T obj)
    {
        context.Add(obj);
        return Save();
    }

    public bool Delete(T obj)
    {
        context.Remove(obj);
        return Save();
    }
    public bool Save()
    {
        var saved = context.SaveChanges();
        return saved > 0 ? true : false;
    }

    public bool Update(T obj)
    {
        context.Update(obj);
        return Save();
    }
}
