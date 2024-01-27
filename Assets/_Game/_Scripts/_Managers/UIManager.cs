using VTemplate.UI;

public class UIManager : Singleton<UIManager>
{

    public DisplayTextUI laughterEmojiText;


    private void Start()
    {

    }
    private void Update()
    {

    }
    public void UpdateLaughterEmoji(int amount)
    {
        laughterEmojiText.SetText(amount.ToString());
    }
}
