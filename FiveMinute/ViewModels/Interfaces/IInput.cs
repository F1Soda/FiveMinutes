namespace FiveMinute.ViewModels.Interfaces;

public interface IInput<TView, T>//из бекэнда в фронт
{
    static abstract TView CreateByModel(T model);
}