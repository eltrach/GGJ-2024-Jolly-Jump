using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace VTemplate.UI
{
    public class DisplayTextUI : MonoBehaviour
    {
        [Title("Progress Bar Settings")]
        [SerializeField] private bool getComponentOnStart = true;

        public TextMeshProUGUI textGUIComponent;

        private void Start()
        {
            if (getComponentOnStart)
            {
                textGUIComponent = GetComponent<TextMeshProUGUI>();
            }
        }
        public void SetText(string text)
        {
            textGUIComponent.text = text;
        }
    }
}