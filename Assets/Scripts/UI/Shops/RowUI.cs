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
        [SerializeField] GameObject quantityTextParent;
        [SerializeField] TextMeshProUGUI quantityTextPrefab;

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

            if (currentShop.GetTransactionQuantity (currentItem.GetInventoryItem ()) <= 0 && currentShop != null)
            {
                quantityField.text = " ";
            }
            else
            {
                quantityField.text = currentShop.GetTransactionQuantity (currentItem.GetInventoryItem ()).ToString ();
            }


            currentItem.OnItemChange += RefreshAvailability;
            Debug.Log("Made it to the end of RowUI Setup");


        }

        public void Add ()
        {
            currentShop.AddToTransaction (currentItem.GetInventoryItem (), 1);
            //RefreshQuantity ();
        }

        public void Remove ()
        {
            currentShop.AddToTransaction (currentItem.GetInventoryItem (), -1);
            //RefreshQuantity ();
        }

        private void RefreshAvailability()
        {
            Debug.Log("refresh Availability UI called");
            availabilityField.text = currentItem.GetAvailability().ToString();
        }

        // private void RefreshQuantity ()
        // {
        //     Destroy (quantityField);

        //     TextMeshProUGUI newQuantityField = Instantiate<TextMeshProUGUI> (quantityTextPrefab, quantityTextParent.transform);
        //     quantityField = newQuantityField;
        //     if (currentShop.GetTransactionQuantity (currentItem.GetInventoryItem ()) <= 0)
        //     {
        //         quantityField.text = " ";
        //     }
        //     else
        //     {
        //         quantityField.text = currentShop.GetTransactionQuantity (currentItem.GetInventoryItem ()).ToString ();
        //     }

        // }

    }
}