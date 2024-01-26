using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace VTemplate.UI
{
    public class ShopWindow : WindowImpl
    {
        [SerializeField] private RectTransform _shopRect;

        public ShopItemUI shopItemTemplate;
        public Transform shopListParent;

        List<ShopItemUI> shopitemsUI;
        public override void Init()
        {
            ClearAnyExistingChildren();
            //foreach (var item in Root.ShopManager.ShopDataManager.shopItems)
            //{
            //    ShopItemUI shopItemUI = Instantiate(shopItemTemplate,shopListParent);
            //    shopItemUI.Init(item.icon, item.Itemname, item.price.ToString(), item.locked);
            //    //if(shopItemUI) shopitemsUI.Add(shopItemUI);

            //}
            _shopRect.anchoredPosition = new Vector2(_shopRect.anchoredPosition.x, 700);
            base.Init();
        }

        private void ClearAnyExistingChildren()
        {
            foreach (Transform item in shopListParent)
            {
                Destroy(item.gameObject);
            }
        }

        protected override void OpenInternal()
        {
            DOTween.Kill(gameObject);
            _shopRect.DOAnchorPosY(0, .35f).SetId(gameObject);
        }

        protected override void CloseInternal()
        {
            base.CloseInternal();
            DOTween.Kill(gameObject);
            _shopRect.DOAnchorPosY(700, .2f).SetId(gameObject);
        }
    }
}