using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VTemplate.UI
{
    public class TransitionScreenWindow : WindowImpl
    {
        [SerializeField] private SplashImageUI _splashImage;
        [SerializeField] private float _fullScreenWaitTime = 0.1f;
        [SerializeField] private float _showSplashTime = 1f;
        [SerializeField] private float _hideSplashTime = 1f;
        public delegate void FullSplashScreen();
        public FullSplashScreen OnFullSplashScreen;
        public delegate void ClosedSplashScreen();
        public ClosedSplashScreen OnClosedSplashScreen;

        private Color _baseColor;
        
        protected override void OpenInternal()
        {
            DoSplash();
        }
        protected override void CloseInternal()
        {
            base.CloseInternal();
            _baseColor = _splashImage.SplashImage.color;
            _splashImage.SplashImage.color = new Color(_baseColor.r, _baseColor.g, _baseColor.b, 0f);
            OnClosedSplashScreen?.Invoke();
        }
        [Button]
        private void DoSplash()
        {
            _splashImage.SplashImage.DOFade(_splashImage.FullSplashAlpha / 255f, _showSplashTime).OnComplete(() =>
            {
                OnFullSplashScreen?.Invoke();
                _splashImage.SplashImage.DOFade(_splashImage.EndSplashAlpha / 255f, _hideSplashTime)
                .SetDelay(_fullScreenWaitTime)
                .OnComplete(Close);
            });
        }
    }


}
