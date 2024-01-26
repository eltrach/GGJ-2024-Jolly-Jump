using UnityEngine;
using DG.Tweening;


namespace VTemplate.UI
{
    public class TutoSwerveUI : MonoBehaviour, ITutoUI
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private TutoTrailUI _trail;



        public void Show ()
        {
            _rect.anchoredPosition = new Vector2(-300, 0);
            MoveHand();
            _trail.Show();
        }

        public void Hide ()
        {
            _trail.Hide();
            _rect.DOKill();
        }

        void MoveHand ()
        {
            _rect.DOAnchorPosX(300, .75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
    }
}