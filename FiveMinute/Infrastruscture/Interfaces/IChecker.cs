using FiveMinute.Models;
using FiveMinute.ViewModels;

namespace FiveMinute.Interfaces;

public interface IChecker
{
    /// <summary>
    /// Возвращает, успешно ли добавлено все в бд
    /// </summary>
    Task<bool> CheckAndSave(TestResultViewModel testResultViewModel);
}