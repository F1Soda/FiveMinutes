namespace FiveMinute.ViewModels.Interfaces;

public interface IInput<out TView, in T>
{
    static abstract TView CreateByModel(T model);
}