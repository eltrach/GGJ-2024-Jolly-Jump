using UnityEngine;

public class TriggerCounter : MonoBehaviour
{
    [SerializeField] private int triggerCount = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
            triggerCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
            triggerCount--;
    }
    private void Update()
    {
        if(triggerCount <= 0)
        {
            // WinState();
            // Root.GameManager.GameWin();
        }
    }
#if none 
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.alignment = TextAnchor.MiddleCenter;

        GUI.Box(new Rect(10, 10, 100, 40), "Triggers: " + triggerCount.ToString(), style);
    }
#endif

}
