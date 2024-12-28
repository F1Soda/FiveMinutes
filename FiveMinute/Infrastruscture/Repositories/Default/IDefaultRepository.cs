using FiveMinute.Data;

namespace FiveMinute.Interfaces;

public interface IDefaultRepository<T>
{
    Task<ResultOperationInDatabase> Add(T fmt);

    Task<ResultOperationInDatabase> Delete(T fmt);

    Task<ResultOperationInDatabase> Save();
}