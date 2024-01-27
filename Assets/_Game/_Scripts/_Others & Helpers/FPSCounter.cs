using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{

    public float updateInterval = 0.5f;

    private float accum = 0.0f;
    private float frames = 0;
    private float timeLeft = 0.0f;

    private float currentFPS = 0.0f;
    private float averageFPS = 0.0f;

    [SerializeField] TextMeshProUGUI textMeshProCurrent;
    [SerializeField] TextMeshProUGUI textMeshProAverage;

    void Start()
    {
        timeLeft = updateInterval;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        if (timeLeft <= 0.0f)
        {
            currentFPS = accum / frames;
            averageFPS = frames / updateInterval;

            timeLeft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
        textMeshProCurrent.text = "Current FPS: " + currentFPS.ToString();
        textMeshProAverage.text = "Average FPS: " + averageFPS.ToString();
    }
#if UNITY_EDITOR
    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.alignment = TextAnchor.MiddleCenter;

        GUI.Label(new Rect(10, 10, 200, 20), "Current FPS: " + currentFPS.ToString(), style);
        GUI.Label(new Rect(10, 30, 200, 20), "Average FPS: " + averageFPS.ToString(), style);
    }
#endif
}
