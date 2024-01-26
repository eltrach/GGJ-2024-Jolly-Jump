using System.Collections;
using UnityEngine;


namespace MWM.PrototypeTemplate
{
    public sealed class KeyboardJoystickControllerImpl : TouchControllerImpl, IJoystickController
    {
        public event JoystickEvent OnJoystickInputReceived;
    
        private bool _touching;
        private Vector2 _joystickPosition;
        private bool _left;
        private bool _right;
        private bool _up;
        private bool _down;
        private bool _any;
    
        
        
        public KeyboardJoystickControllerImpl (MonoBehaviour mono)
        {
            Precondition.CheckNotNull(mono);
            mono.StartCoroutine(UpdateCoroutine());
        }
    
        protected override IEnumerator UpdateCoroutine ()
        {
            while (true)
            {
                _left = Input.GetKey(KeyCode.LeftArrow);
                _right = Input.GetKey(KeyCode.RightArrow);
                _up = Input.GetKey(KeyCode.UpArrow);
                _down = Input.GetKey(KeyCode.DownArrow);
                _any = _left || _right || _up || _down;
    
                if (!_touching && _any) 
                    DoOnTouchStart();
                if (_touching && !_any) 
                    DoOnTouching();
                if (_any) 
                    DoOnTouchEnd();
    
                _touching = _any;
    
                _joystickPosition.x = _joystickPosition.y = 0;
                if (_left)
                    _joystickPosition.x -= 1;
                if (_right)
                    _joystickPosition.x += 1;
                if (_up)
                    _joystickPosition.y += 1;
                if (_down)
                    _joystickPosition.y -= 1;
                if (_joystickPosition.x != 0 || _joystickPosition.y != 0)
                {
                    _joystickPosition.Normalize();
                    OnJoystickInputReceived?.Invoke(_joystickPosition);
                }
    
                yield return null;
            }
        }
        
        protected override void DoOnTouchStart ()
        {
            CallOnTouchStart();
        }

        protected override void DoOnTouching ()
        {
            CallOnTouching();
        }

        protected override void DoOnTouchEnd ()
        {
            CallOnTouchEnd();
        }
    }
}