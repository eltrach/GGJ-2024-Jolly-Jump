using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace VTemplate.UI
{
    public class WinWindow : WindowImpl
    {
        [SerializeField] private RectTransform _winRect;
        [SerializeField] private ButtonUI _nextButton;
        public delegate void ClosedWindow();
        public ClosedWindow OnClosedWinWindow;

        // SubReward
        public GameObject _subRewardPanel;

        public ProgressBarUI subRewardSlider;

        [PreviewField]
        public Sprite[] shoes;

        public Image background;
        public Image fill;
        public ButtonUI _claimButton;

        public override void Init()
        {
            base.Init();
            _nextButton.Init(NextLevel);
            _claimButton.Init(ClaimReward);
            _claimButton.gameObject.SetActive(false);

            subRewardSlider.UpdateProgress(0);

            _winRect.anchoredPosition = new Vector2(_winRect.anchoredPosition.x, 700);
            _nextButton.Rect.localScale = Vector3.zero;

            if (IsOutOfRange(GlobalRoot.DataManager.ShoesIndex))
            {
                Debug.Log("Out of range exception");
                _subRewardPanel.SetActive(false);
                _claimButton.gameObject.SetActive(false);
            }
            else ChangeSubRewardSprites(GlobalRoot.DataManager.ShoesIndex);
        }
        private void ClaimReward()
        {
            int shoesIndex = GlobalRoot.DataManager.ShoesIndex;
            shoesIndex += 1;
            GlobalRoot.DataManager.SubRewardProgression = 0;

            if (IsOutOfRange(shoesIndex))
            {
                //int randomIndex = Random.Range(0, shoes.Length - 1);
                //Root.DataManager.RandomSelectedShoesIndex = randomIndex;
                //Debug.Log("selecting a random shoes index : " + randomIndex);
                //ChangeSubRewardSprites(randomIndex);
                //Root.GameManager.PlayerLegC.leg.ChooseShoesByIndex(randomIndex);

                Debug.Log("Out of range exception" + shoesIndex);
                _subRewardPanel.SetActive(false);
                _claimButton.gameObject.SetActive(false);
                GlobalRoot.DataManager.ShoesIndex = shoesIndex;
            }
            else
            {
                GlobalRoot.DataManager.ShoesIndex = shoesIndex;
                ChangeSubRewardSprites(shoesIndex);
            }
            subRewardSlider.UpdateProgress(0);
            _claimButton.gameObject.SetActive(false);
            NextLevel();
        }

        private bool IsOutOfRange(int shoesIndex)
        {
            Debug.Log("count " + shoes.Length + "shoesIndex" + shoesIndex);
            return shoesIndex < 0 || shoesIndex >= shoes.Length;
        }

        protected override void OpenInternal()
        {
            DOTween.Kill(gameObject);
            _winRect.DOAnchorPosY(0, .35f).SetId(gameObject);
            _nextButton.Rect.DOScale(1, .35f).SetEase(Ease.OutBack).SetId(gameObject).OnComplete(SetWindowOpenedState);

            if (IsOutOfRange(GlobalRoot.DataManager.ShoesIndex))
            {
                _subRewardPanel.SetActive(false);
                _claimButton.gameObject.SetActive(false);
                return;
            }

            // data 
            if (GlobalRoot.DataManager.SubRewardProgression <= 1)
            {
                GlobalRoot.DataManager.SubRewardProgression += 0.2f;
                SubRewardSliderUpdate(GlobalRoot.DataManager.SubRewardProgression);
            }
            if (GlobalRoot.DataManager.SubRewardProgression >= 1)
            {
                _claimButton.gameObject.SetActive(true);
            }
        }

        protected override void CloseInternal()
        {
            DOTween.Kill(gameObject);
            _winRect.DOAnchorPosY(700, .2f).SetId(gameObject);
            _nextButton.Rect.DOScale(0, .2f).SetId(gameObject).OnComplete(SetWindowClosedState);
            OnClosedWinWindow?.Invoke();
        }
        private void NextLevel()
        {
            UIManager.Instance.transitionScreenWindow.Open();
            Close();
        }
        public void SubRewardSliderUpdate(float newValue)
        {
            subRewardSlider.UpdateProgress(newValue);
        }
        public void ChangeSubRewardSprites(int shoesIndex)
        {
            background.sprite = shoes[shoesIndex];
            fill.sprite = shoes[shoesIndex];
        }

    }

}