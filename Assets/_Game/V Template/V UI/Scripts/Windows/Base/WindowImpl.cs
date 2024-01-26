using UnityEngine;
using Sirenix.OdinInspector;

namespace VTemplate.UI
{
    public abstract class WindowImpl : MonoBehaviour, IWindow
    {
        public event WindowEvents OnVisibilityStatusChanged;

        public WindowVisibilityStatus VisibilityStatus { get; private set; }

        [SerializeField, Required, BoxGroup("Window")] protected Canvas _canvas = null;
        [SerializeField, Required, BoxGroup("Window")] protected CanvasGroup _canvasGroup = null;

        private bool _interactable = false;

        public virtual void Init()
        {
            _interactable = _canvasGroup.interactable;
            SetWindowClosedState();
        }

        public void Open()
        {
            if (VisibilityStatus == WindowVisibilityStatus.Closed)
            {
                _canvas.enabled = true;
                ChangeVisibilityStatus(WindowVisibilityStatus.Opening);
                OpenInternal();
            }
        }

        public void Close()
        {
            if (VisibilityStatus == WindowVisibilityStatus.Opening || VisibilityStatus == WindowVisibilityStatus.Opened)
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                ChangeVisibilityStatus(WindowVisibilityStatus.Closing);
                CloseInternal();
            }
        }

        protected virtual void OpenInternal()
        {
            SetWindowOpenedState();
        }
        protected virtual void CloseInternal()
        {
            SetWindowClosedState();
        }
        protected void SetWindowOpenedState()
        {
            _canvasGroup.interactable = _interactable;
            _canvasGroup.blocksRaycasts = true;
            ChangeVisibilityStatus(WindowVisibilityStatus.Opened);
        }
        protected void SetWindowClosedState()
        {
            _canvas.enabled = false;
            _canvasGroup.enabled = false;
            ChangeVisibilityStatus(WindowVisibilityStatus.Closed);
        }
        private void ChangeVisibilityStatus(WindowVisibilityStatus newStatus)
        {
            VisibilityStatus = newStatus;
            OnVisibilityStatusChanged?.Invoke(this);
        }
    }
}
