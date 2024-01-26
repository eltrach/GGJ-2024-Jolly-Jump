using System;
using UnityEngine;
using UnityEngine.UI;

namespace VTemplate.UI
{
    public class ButtonUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private Button _button;

        public RectTransform Rect => _rect;

        public void Init(Action callback)
        {
            Precondition.CheckNotNull(callback);
            _button.onClick.AddListener(() => callback());
        }
    }
}