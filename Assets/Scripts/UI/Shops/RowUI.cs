using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class RowUI : MonoBehaviour
    {

        [SerializeField] Image iconField;
        [SerializeField] TextMeshProUGUI nameField;
        [SerializeField] TextMeshProUGUI availabilityField;
        [SerializeField] TextMeshProUGUI priceField;
        [SerializeField] TextMeshProUGUI quantityField;
        [SerializeField] Button minusButton = null;
        [SerializeField] Button plusButton = null;
        Shop currentShop = null;
        ShopItem currentItem = null;

        public void Setup (Shop shop, ShopItem item)
        {
            nameField.text = item.GetName ();
            iconField.sprite = item.GetIcon ();
            availabilityField.text = $"{ item.GetAvailability()}";
            priceField.text = $"{ item.getPrice():N0}";

            currentShop = shop;
            currentItem = item;

            plusButton.interactable = currentShop.HasInventorySpace ();

            if (currentShop.GetTransactionQuantity (currentItem.GetInventoryItem ()) <= 0 && currentShop != null)
            {
                quantityField.text = " ";
            }
            else
            {
                quantityField.text = currentShop.GetTransactionQuantity (currentItem.GetInventoryItem ()).ToString ();
                RefreshCount ();
            }

            currentItem.OnItemChange += RefreshAvailability;
            currentShop.OnChange += RefreshCount;
        }

        public void Add ()
        {
            currentShop.AddToTransaction (currentItem.GetInventoryItem (), 1);
        }

        public void Remove ()
        {
            currentShop.AddToTransaction (currentItem.GetInventoryItem (), -1);
        }

        private void RefreshAvailability ()
        {
            availabilityField.text = currentItem.GetAvailability ().ToString ();
        }

        private void RefreshCount ()
        {

            if (!currentShop.HasInventorySpace ())
            {
                quantityField.color = Color.red;
            }

            if (currentShop.HasInventorySpace ())
            {
                quantityField.color = Color.white;
            }
        }
    }
}