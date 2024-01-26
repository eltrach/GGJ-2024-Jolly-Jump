using TMPro;
using UnityEngine;


public class ActionUIPanel : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public GameObject panel;

    private void Start()
    {
        Disable();
    }
    public void Enable(string messageOnTriggerEnter)
    {
        panel.SetActive(true);
        messageText.text = messageOnTriggerEnter;
    }

    public void Disable()
    {
        panel.SetActive(false);
        messageText.text = "";
    }
}
