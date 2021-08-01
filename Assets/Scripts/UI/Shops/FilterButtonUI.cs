using System;
using GameDevTV.Inventories;
using RPG.Shops;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class FilterButtonUI : MonoBehaviour
    {
        [SerializeField] ItemCategory category = ItemCategory.None;

        Button button;
        Shop currentShop;
        private void Awake ()
        {
            button = GetComponent<Button> ();
            button.onClick.AddListener (SelectFilter);
        }

        public void SetShop (Shop shop)
        {
            currentShop = shop;
        }

        public void refreshUI ()
        {
            button.interactable = category != currentShop.GetFilter ();
        }

        private void SelectFilter ()
        {
            currentShop.SelectFilter (category);
        }

    }
}