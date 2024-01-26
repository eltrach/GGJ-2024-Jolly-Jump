namespace MWM.PrototypeTemplate
{
    public delegate void TouchEvent();
    
    
    public interface ITouchController
    {
        event TouchEvent OnTouchStart;
        event TouchEvent OnTouching;
        event TouchEvent OnTouchEnd;
    }
}