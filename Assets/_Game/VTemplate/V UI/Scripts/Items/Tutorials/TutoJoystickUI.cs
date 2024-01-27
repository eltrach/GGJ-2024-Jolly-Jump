using System.Collections;
using UnityEngine;


namespace VTemplate.UI
{
    public class TutoJoystickUI : MonoBehaviour, ITutoUI
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private float _radius = 200;
        [SerializeField] private float _speed = 1.5f;

        private Coroutine _moveRoutine;
        


        public void Show ()
        {
            _moveRoutine = StartCoroutine(MoveRoutine());
        }

        public void Hide ()
        {
            if (_moveRoutine == null)
                return;
            StopCoroutine(_moveRoutine);
        }
        
        IEnumerator MoveRoutine ()
        {
            while (true)
            {
                float t = Time.time * Mathf.PI * _speed;
                float x = _radius * Mathf.Cos(t) / (1 + Mathf.Sin(t) * Mathf.Sin(t));
                float y = _radius * Mathf.Sin(t) * Mathf.Cos(t) / (1 + Mathf.Sin(t) * Mathf.Sin(t));
                _rect.anchoredPosition = new Vector2(x, y);
                yield return null;
            }
        }
    }
}