using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VTemplate.UI
{
    public class ShopItemUI : MonoBehaviour
    {
        public Image itemIcon;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI priceText;
        public bool locked;

        public void Init(Sprite icon ,string itemName,string price, bool locked)
        {
            itemIcon.sprite = icon;
            itemNameText.text = itemName;
            priceText.text = price;
        }
        
    }
}