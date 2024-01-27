namespace VTemplate.UI
{
    public delegate void WindowEvents(IWindow window);
    public enum WindowVisibilityStatus { Opening, Opened, Closing, Closed };



    public interface IWindow
    {
        event WindowEvents OnVisibilityStatusChanged;

        WindowVisibilityStatus VisibilityStatus { get; }

        void Init();
        void Open();
        void Close();
    }
}