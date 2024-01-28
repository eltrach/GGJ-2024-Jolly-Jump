using System.Collections;
using TMPro;
using UnityEngine;

public class LoseTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdownDuration = 5f;
    public void Init()
    {
        StartCoroutine(StartCountdown());
    }
    IEnumerator StartCountdown()
    {
        float timer = countdownDuration;

        while (timer > 0)
        {
            countdownText.text = "Reloading level in " + timer.ToString("0") + " seconds";
            yield return new WaitForSeconds(1f);
            timer--;
        }
    }
}
