using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VTemplate.UI;

public class UIManager : Singleton<UIManager>
{
    [Title("Windows")]
    public TransitionScreenWindow transitionScreenWindow;
    public LoseWindow loseWindow;
    public WinWindow winWindow;
    public ShopWindow shopWindow;
    public TutoJoystickUI tutoHandMover;

    [Space]
    [Title("HeaderComp")]
    public DisplayTextUI currentLevelText;
    public DisplayTextUI coinsTextUI;
    public ProgressBarDistanceChecker distanceChecker; // level progressbar updater

    public ButtonUI openShopBtn;
    public ButtonUI closeShopBtn;

    CanvasGroup tutoCanvasGroup;
    CurrencyManager currencyManager;
    ShopManager shopManager;

    [Button(ButtonSizes.Large)]
    [FoldoutGroup("Initialization")]
    [InfoBox("Initialize the UIManager.")]
    public void Init()
    {
        Debug.Log("UIManager initialized.");
        // get comp
        currencyManager = GetComponent<CurrencyManager>();

        ShowHandTuto();
        loseWindow.Init();
        winWindow.Init();
        transitionScreenWindow.Init();
        UpdateCoinsUI();
        InitShop();
        openShopBtn.Init(OpenShop);
        closeShopBtn.Init(CloseShop);
        //GameManager.Instance.canPlay = true;
        //
        winWindow.OnVisibilityStatusChanged += WinWindowVisibilityChanged;
        loseWindow.OnVisibilityStatusChanged += LoseWindowVisibilityChanged;
    }
    //Shop
    private void InitShop()
    {
        shopWindow.Init();
    }
    private void OpenShop()
    {
        shopWindow.Open();
        GameManager.Instance.CanPlay = false;
    }
    [Button]
    private void CloseShop()
    {
        shopWindow.Close();
        GameManager.Instance.CanPlay = true;
    }
    public void UpdateLevelText(int newLevel)
    {
        if (currentLevelText == null)
        {
            Debug.Log("currentLevelText Is Null");
            return;
        }
        currentLevelText.SetText((newLevel + 1).ToString());
    }
    public void UpdateCoinsUI()
    {
        int Coins = currencyManager.GetCoins();
        coinsTextUI.SetText(Coins + "$");
    }

    public void GameStart()
    {
        HideHandTuto();
    }
    public void HideHandTuto()
    {
        tutoHandMover.Hide();
        tutoCanvasGroup.DOFade(0, 0.4f).OnComplete(() =>
        {
            tutoHandMover.gameObject.SetActive(false);
        });
    }
    public void ShowHandTuto()
    {
        tutoHandMover.gameObject.SetActive(true);
        tutoCanvasGroup.alpha = 1;
        tutoHandMover.Show();
    }
    [Button(ButtonSizes.Large)]
    [FoldoutGroup("Game Over")]
    [InfoBox("Display the game over panel.")]
    public void GameLose()
    {
        loseWindow.Open();
        GameManager.Instance.CanPlay = false;
    }

    [Button(ButtonSizes.Large)]
    [FoldoutGroup("Game Over")]
    [InfoBox("Hide the win panel.")]
    public void GameWin()
    {
        winWindow.Open();
        GameManager.Instance.CanPlay = false;
    }
    private void WinWindowVisibilityChanged(IWindow window)
    {
        switch (window.VisibilityStatus)
        {
            case WindowVisibilityStatus.Closing:
                transitionScreenWindow.Open();
                break;
            case WindowVisibilityStatus.Closed:
                GlobalRoot.NextLevel();
                GameManager.Instance.CanPlay = true;
                break;
        }
    }
    private void LoseWindowVisibilityChanged(IWindow window)
    {
        switch (window.VisibilityStatus)
        {
            case WindowVisibilityStatus.Closing:
                transitionScreenWindow.Open();
                break;
            case WindowVisibilityStatus.Closed:
                GlobalRoot.ReloadLevel();
                GameManager.Instance.CanPlay = true;
                break;
        }
    }
}
