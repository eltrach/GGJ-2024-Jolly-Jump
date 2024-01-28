using DG.Tweening;
using UnityEngine;
using VTemplate.UI;

public class UIManager : Singleton<UIManager>
{

    public DisplayTextUI laughterEmojiText;

    public GameObject winScreen;
    public NextLevelTimer nextLevelTimer;

    public GameObject loseScreen;
    public LoseTimer loseTimer;


    public GameObject collectMoreEmojis;
    public float collectMoreTimer = 2f;

    private void Start()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }
    public void UpdateLaughterEmoji(int amount)
    {
        laughterEmojiText.SetText(amount.ToString());
    }
    public void EnableCollectMoreEmojis()
    {
        DOTween.KillAll(false);
        collectMoreEmojis.transform.localScale = Vector3.one;
        collectMoreEmojis.SetActive(true);
        collectMoreEmojis.transform.DOScale(1.2f, 0.3f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            collectMoreEmojis.SetActive(false);
        });
        //yield return new WaitForSeconds(collectMoreTimer);
    }

    public void LoadWinScreen()
    {
        winScreen.SetActive(true);
        if (winScreen.activeInHierarchy) nextLevelTimer.Init();
    }
    public void LoadLoseScreen()
    {
        loseScreen.SetActive(true);
        loseTimer.Init();
    }
}
