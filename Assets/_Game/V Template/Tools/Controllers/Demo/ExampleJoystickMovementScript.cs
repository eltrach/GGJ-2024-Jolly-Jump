using UnityEngine;


namespace MWM.PrototypeTemplate
{
    public class ExampleJoystickMovementScript : MonoBehaviour
    {
        [SerializeField] private float _speed = 3;
        
        private IJoystickController _joystickController;

        
        
        private void Awake ()
        {
            _joystickController = new JoystickControllerImpl(this);
            _joystickController.OnJoystickInputReceived += Move;
            _joystickController.OnTouchStart += () => { Debug.Log("Touch Start"); };
            _joystickController.OnTouching += () => { Debug.Log("Touching"); };
            _joystickController.OnTouchEnd += () => { Debug.Log("Touch End"); };
        }
        private void Move (Vector2 direction)
        {
            transform.Translate(new Vector3(direction.x, 0, direction.y) * _speed * Time.deltaTime);
        }
    }
}