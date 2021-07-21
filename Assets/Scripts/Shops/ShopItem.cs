using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Shops
{
    public class ShopItem
    {
        InventoryItem item;
        int availability;
        float price;
        int quantityinTransaction;

        public event Action OnItemChange;

        public int QuantityInTransaction { get { return quantityinTransaction; } set { quantityinTransaction = value; } }

        public ShopItem (InventoryItem item, int availability, float price, int quantityinTransaction)
        {
            this.item = item;
            this.availability = availability;
            this.price = price;
            this.quantityinTransaction = quantityinTransaction;
        }

        public InventoryItem GetInventoryItem ()
        {
            return item;
        }

        public Sprite GetIcon ()
        {
            return item.GetIcon ();
        }

        public string GetName ()
        {
            return item.GetDisplayName ();
        }

        public int GetAvailability ()
        {
            return availability;
        }

        public void SetAvailability(ShopItem item, int amount)
        {
            item.availability += amount;
            Debug.Log("Set Availability called on " + item + " availability changed to " + amount);

            if (OnItemChange != null )
            {
                Debug.Log("OnItemChange transmitted");
                OnItemChange();
            }
        }

        public float getPrice ()
        {
            return price;
        }

        public int GetQuantityInTransaction()
        {
            return quantityinTransaction;
        }

    }
}