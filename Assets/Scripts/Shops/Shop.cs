using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.Inventories;
using RPG.Control;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable
    {

        [SerializeField] string shopName = null;

        [SerializeField] StockItemConfig[] stockConfig;

        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range (0, 50)] public float buyingdiscountPercentage;
        }

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int> ();
        Dictionary<InventoryItem, int> stock = new Dictionary<InventoryItem, int> ();
        Shopper currentShopper = null;
        bool canTransact = true;

        public event Action OnChange;

        private void Awake ()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                stock[config.item] = config.initialStock;
            }
        }

        public void SetShopper (Shopper shopper)
        {
            currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetAllItems ()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                int quantityinTransaction = 0;
                transaction.TryGetValue (config.item, out quantityinTransaction);
                int currentStock = stock[config.item];
                yield return new ShopItem (config.item, currentStock, AdjustedPrice (config), quantityinTransaction);
            }
        }

        public IEnumerable<ShopItem> GetFilteredItems ()
        {
            return GetAllItems ();
        }

        public string GetShopName ()
        {
            if (shopName == null)
            {
                return "Null Name";
            }
            return shopName;
        }

        public void AddToTransaction (InventoryItem item, int quantity)
        {

            if (!transaction.ContainsKey (item))
            {
                transaction[item] = 0;
            }

            if (transaction[item] + quantity > stock[item])
            {
                transaction[item] = stock[item];
            }
            else
            {
                transaction[item] += quantity;
            }

            if (transaction[item] <= 0)
            {
                transaction.Remove (item);
            }

            if (OnChange != null)
            {
                OnChange ();
            }
        }

        public int GetTransactionQuantity (InventoryItem item)
        {
            if (!transaction.ContainsKey (item))
            {
                return 0;
            }
            else
            {
                return transaction[item];
            }
        }

        public void SelectFilter (ItemCategory category)
        {

        }

        public ItemCategory GetFilter ()
        {
            return ItemCategory.None;
        }

        public void SelectMode (bool isBuying)
        {

        }

        public bool IsBuyingMode ()
        {
            return true;
        }

        public bool CanTransact ()
        {
            return canTransact;
        }

        public void ConfirmTransaction ()
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory> ();
            Purse shopperPurse = currentShopper.GetComponent<Purse> ();

            if (shopperInventory == null || shopperPurse == null) return;

            foreach (ShopItem shopItem in GetAllItems ())
            {
                InventoryItem item = shopItem.GetInventoryItem ();
                int quantity = shopItem.GetQuantityInTransaction ();
                float price = shopItem.getPrice ();

                for (var i = 0; i < quantity; i++)
                {
                    if (shopperPurse.GetBalance () < price) break;

                    bool success = shopperInventory.AddToFirstEmptySlot (item, 1);
                    bool hasStock = shopItem.GetAvailability () > 0;

                    if (success && hasStock)
                    {
                        AddToTransaction (item, -1);
                        stock[item]--;
                        shopperPurse.UpdateBalance (-price);
                    }
                }
            }

            if (OnChange != null)
            {
                OnChange ();
            }
        }

        public float GetTransactionTotal ()
        {
            float transactionTotal = 0;

            foreach (ShopItem item in GetAllItems ())
            {
                if (transaction.ContainsKey (item.GetInventoryItem ()))
                {
                    transactionTotal += item.getPrice () * transaction[item.GetInventoryItem ()];
                }
            }

            return transactionTotal;
        }

        public CursorType GetCursorType ()
        {
            return CursorType.Shop;
        }

        public bool HandleRaycast (PlayerController callingController)
        {
            if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1))
            {
                callingController.GetComponent<Shopper> ().SetActiveShop (this);
            }

            return true;
        }

        private float AdjustedPrice (StockItemConfig config)
        {
            if (config.buyingdiscountPercentage != 0)
            {
                return config.item.GetItemPrice () - (config.item.GetItemPrice () * (config.buyingdiscountPercentage * 0.01f));
            }

            else
            {
                return config.item.GetItemPrice ();
            }
        }

    }
}