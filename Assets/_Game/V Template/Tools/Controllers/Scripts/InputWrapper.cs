using UnityEngine;
using UnityEngine.EventSystems;


namespace MWM.PrototypeTemplate
{
    public static class InputWrapper
    {
        public static Vector2 Position => Input.mousePosition;
        public static bool TouchStart => Input.GetMouseButtonDown(0) || Input.touchCount != 0 && Input.touches[0].phase == TouchPhase.Began;
        public static bool Touching => Input.GetMouseButton(0) || Input.touchCount != 0 && Input.touches[0].phase != TouchPhase.Ended;
        public static bool TouchEnd => Input.GetMouseButtonUp(0) || Input.touchCount != 0 && Input.touches[0].phase == TouchPhase.Ended;

        public static bool NonUiTouchStart => TouchStart && !OverUiElements;
        public static bool NonUiTouching => Touching && !OverUiElements;
        public static bool NonUiTouchEnd => TouchEnd && !OverUiElements;

        private static bool _eventSystemActivated;
    
        public static bool OverUiElements
        {
            get
            {
                if (!_eventSystemActivated)
                {
                    if (EventSystem.current == null)
                        return false;
                    _eventSystemActivated = true;
                }
                return Input.touchCount != 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) || Input.touchCount == 0 && EventSystem.current.IsPointerOverGameObject();
            }
        }
    }
}