namespace FiveMinute.ViewModels.Interfaces;

public interface IInput<out TView, in T>//из бекэнда в фронт
{
    static abstract TView CreateByModel(T model);
}