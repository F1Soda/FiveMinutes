namespace FiveMinute.ViewModels.Interfaces;

public interface IOutput<in TView, out T>//из фронта в бекенд
{
    static abstract T CreateByView(TView model);
}