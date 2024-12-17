namespace FiveMinute.ViewModels.Interfaces;

public interface IOutput<in TView, out T>
{
    static abstract T CreateByView(TView model);
}