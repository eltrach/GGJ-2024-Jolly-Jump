using UnityEngine;


namespace MWM.PrototypeTemplate
{
    public delegate void JoystickEvent(Vector2 joystickInput);
    
    
    public interface IJoystickController : ITouchController
    {
        event JoystickEvent OnJoystickInputReceived;
    }
}