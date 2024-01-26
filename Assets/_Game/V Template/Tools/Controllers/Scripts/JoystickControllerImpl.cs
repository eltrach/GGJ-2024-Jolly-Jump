using UnityEngine;


namespace MWM.PrototypeTemplate
{
    public sealed class JoystickControllerImpl : TouchControllerImpl, IJoystickController
    {
        public event JoystickEvent OnJoystickInputReceived;

        private readonly float _radius;
        private Vector2 _centerPosition;
        private Vector2 _joystickPosition;
        private float _magnitude;



        public JoystickControllerImpl (MonoBehaviour mono) : this(mono, Screen.width * .1f)
        {
            
        }
        
        public JoystickControllerImpl (MonoBehaviour mono, float radius)
        {
            Precondition.CheckNotNull(mono);
            Precondition.CheckNotNull(radius);
            
            _radius = radius;

#if UNITY_EDITOR
            IJoystickController keyboardJoystickController = new KeyboardJoystickControllerImpl(mono);
            keyboardJoystickController.OnJoystickInputReceived += pos => OnJoystickInputReceived?.Invoke(pos);
            keyboardJoystickController.OnTouchStart += DoOnTouchStart;
            keyboardJoystickController.OnTouching += DoOnTouching;
            keyboardJoystickController.OnTouchEnd += DoOnTouchEnd;
#endif
            
            mono.StartCoroutine(UpdateCoroutine());
        }


        protected override void DoOnTouchStart ()
        {
            _centerPosition = InputWrapper.Position;
        }
        
        protected override void DoOnTouching ()
        {
            _joystickPosition = (InputWrapper.Position - _centerPosition) / _radius;
            _magnitude = _joystickPosition.magnitude;
            if (_magnitude > 1)
            {
                _joystickPosition.Normalize();
                _centerPosition = InputWrapper.Position - _joystickPosition * _radius;
            }
            OnJoystickInputReceived?.Invoke(_joystickPosition);
        }

        protected override void DoOnTouchEnd ()
        {
            _joystickPosition = Vector2.zero;
        }
    }
}