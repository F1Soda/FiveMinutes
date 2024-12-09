namespace FiveMinute.ViewModels.Interfaces;

public interface IOutput<TView,T>//из фронта в бекенд
{
    static abstract T CreateByView(TView model);
}