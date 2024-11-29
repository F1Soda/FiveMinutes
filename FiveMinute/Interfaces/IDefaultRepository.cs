namespace FiveMinute.Interfaces;

public interface IDefaultRepository<T>
{
    Task<bool> Add(T fmt);


    Task<bool> Delete(T fmt);

    Task<bool> Save();
}