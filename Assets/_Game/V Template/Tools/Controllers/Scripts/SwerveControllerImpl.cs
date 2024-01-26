using UnityEngine;


namespace MWM.PrototypeTemplate
{
    public sealed class SwerveControllerImpl : TouchControllerImpl, ISwerveController
    {
        public event SwerveEvent OnSwerveInputReceived;

        private float _previousPositionX;
        private float _positionX;
        private float _deltaPercentage;

        
        
        public SwerveControllerImpl (MonoBehaviour mono)
        {
            Precondition.CheckNotNull(mono);
            
#if UNITY_EDITOR
            IJoystickController keyboardJoystickController = new KeyboardJoystickControllerImpl(mono);
            keyboardJoystickController.OnJoystickInputReceived += pos => OnSwerveInputReceived?.Invoke(Mathf.Sign(pos.x) * .017f);
            keyboardJoystickController.OnTouchStart += DoOnTouchStart;
            keyboardJoystickController.OnTouching += DoOnTouching;
            keyboardJoystickController.OnTouchEnd += DoOnTouchEnd;
#endif

            mono.StartCoroutine(UpdateCoroutine());
        }
        
        

        protected override void DoOnTouchStart ()
        {
            _previousPositionX = InputWrapper.Position.x;
        }
        
        protected override void DoOnTouching ()
        {
            _positionX = InputWrapper.Position.x;
            _deltaPercentage = (_positionX - _previousPositionX) / Screen.width;
            OnSwerveInputReceived?.Invoke(_deltaPercentage);
            _previousPositionX = _positionX;
        }

        protected override void DoOnTouchEnd ()
        {
            
        }
    }
}