public interface IPopup
{
    void Open();
    void Close();
    void SetData<T>(T t) where T : class;
}
