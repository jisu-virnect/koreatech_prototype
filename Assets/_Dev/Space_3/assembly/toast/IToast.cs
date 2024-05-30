using System;

public interface IToast
{
    void Show(string message, float duration = 0f, Action act = null);

}