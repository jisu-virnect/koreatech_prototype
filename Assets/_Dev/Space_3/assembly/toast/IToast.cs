using System;

public interface IToast
{
    void ShowHide(string message, float duration = 0f, Action act = null);
    void SetData<T>(T t) where T : class;

}