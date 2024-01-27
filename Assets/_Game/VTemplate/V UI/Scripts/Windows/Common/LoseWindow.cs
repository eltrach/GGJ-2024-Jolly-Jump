using UnityEngine;
using DG.Tweening;

namespace VTemplate.UI
{
    public class LoseWindow : WindowImpl
    {
        [SerializeField] private RectTransform _loseRect;
        [SerializeField] private ButtonUI _retryButton;

        public override void Init()
        {
            base.Init();
            _retryButton.Init(Retry);
            _loseRect.anchoredPosition = new Vector2(_loseRect.anchoredPosition.x, 700);
            _retryButton.Rect.localScale = Vector3.zero;
        }

        protected override void OpenInternal()
        {
            DOTween.Kill(gameObject);
            _loseRect.DOAnchorPosY(0, .35f).SetId(gameObject);
            _retryButton.Rect.DOScale(1, .35f).SetEase(Ease.OutBack).SetId(gameObject).OnComplete(SetWindowOpenedState);
        }

        protected override void CloseInternal()
        {
            DOTween.Kill(gameObject);
            _loseRect.DOAnchorPosY(700, .2f).SetId(gameObject);
            _retryButton.Rect.DOScale(0, .2f).SetId(gameObject).OnComplete(SetWindowClosedState);
        }
        private void Retry()
        {
            Close();
        }
    }
}