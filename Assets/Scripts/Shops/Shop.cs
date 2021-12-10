using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using RPG.Control;
using RPG.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable, ISaveable
    {

        [SerializeField] string shopName = null;

        [SerializeField] StockItemConfig[] stockConfig;

        [SerializeField][Range (0, 75)] float sellingDisadvantage;

        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range (0, 50)] public float buyingdiscountPercentage;
            public int levelToUnlock = 0;
        }

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int> ();
        Dictionary<InventoryItem, int> stockSold = new Dictionary<InventoryItem, int> ();

        Shopper currentShopper = null;
        Inventory shopperInventory = null;
        bool isBuyingMode = true;
        ItemCategory filter = ItemCategory.None;

        public event Action OnChange;

        public void SetShopper (Shopper shopper)
        {
            currentShopper = shopper;
            shopperInventory = currentShopper.GetComponent<Inventory> ();
        }

        public IEnumerable<ShopItem> GetAllItems ()
        {
            Dictionary<InventoryItem, float> prices = GetPrices ();
            Dictionary<InventoryItem, int> availabilities = GetAvailabilities ();

            foreach (InventoryItem item in availabilities.Keys)
            {
                if (availabilities[item] <= 0) continue;

                float price = prices[item];
                int availability = availabilities[item];
                int quantityinTransaction = 0;
                transaction.TryGetValue (item, out quantityinTransaction);
                yield return new ShopItem (item, availability, price, quantityinTransaction);
            }
        }

        public IEnumerable<ShopItem> GetFilteredItems ()
        {

            foreach (ShopItem shopItem in GetAllItems ())
            {
                InventoryItem item = shopItem.GetInventoryItem ();
                if (filter == item.GetCategory () || filter == ItemCategory.None)
                {
                    yield return shopItem;
                }
            }
        }

        public string GetShopName ()
        {
            if (shopName == null)
            {
                return "Null Name";
            }
            else
            {
                return shopName;
            }
            
        }

        public void AddToTransaction (InventoryItem item, int quantity)
        {

            if (!transaction.ContainsKey (item))
            {
                transaction[item] = 0;
            }

            var availabilities = GetAvailabilities ();
            int availability = availabilities[item];
            if (transaction[item] + quantity > availability)
            {
                transaction[item] = availability;
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
            filter = category;

            if (OnChange != null)
            {
                OnChange ();
            }

            GetFilteredItems ();

        }

        public ItemCategory GetFilter ()
        {
            return filter;
        }

        public void SelectMode (bool isBuying)
        {
            isBuyingMode = isBuying;

            if (OnChange != null)
            {
                OnChange ();
            }
        }

        public bool IsBuyingMode ()
        {
            return isBuyingMode;
        }

        public bool CanTransact ()
        {
            if (IsTransactionEmpty ()) return false;
            if (!HasSuccientFunds ()) return false;
            if (!HasInventorySpace ()) return false;

            return true;
        }

        public bool IsTransactionEmpty ()
        {
            return transaction.Count == 0;
        }

        public bool HasSuccientFunds ()
        {
            if (!isBuyingMode) return true;

            Purse currentPurse = currentShopper.GetComponent<Purse> ();
            if (currentPurse == null) return false;

            return currentPurse.GetBalance () >= GetTransactionTotal ();

        }

        public bool HasInventorySpace ()
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory> ();
            if (!isBuyingMode) return true;

            if (shopperInventory == null) return false;

            List<InventoryItem> itemList = new List<InventoryItem> ();

            foreach (ShopItem shopItem in GetAllItems ())
            {
                InventoryItem item = shopItem.GetInventoryItem ();
                int quantity = shopItem.GetQuantityInTransaction ();

                for (var i = 0; i < quantity; i++)
                {
                    itemList.Add (item);
                }
            }
            return shopperInventory.HasSpaceForAll (itemList);
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
                    if (isBuyingMode)
                    {
                        BuyItem (shopperInventory, shopperPurse, shopItem, item, price);
                    }
                    else
                    {
                        SellItem (shopperInventory, shopperPurse, shopItem, item, price);
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

        private int CountItemsInInventory (InventoryItem item)
        {
            int count = 0;
            if (shopperInventory == null) return 0;

            for (var i = 0; i < shopperInventory.GetSize (); i++)
            {
                if (item == shopperInventory.GetItemInSlot (i))
                {
                    count += shopperInventory.GetNumberInSlot (i);
                }
            }
            return count;
        }

        private Dictionary<InventoryItem, float> GetPrices ()
        {
            Dictionary<InventoryItem, float> prices = new Dictionary<InventoryItem, float> ();

            {
                foreach (StockItemConfig config in GetAvailableConfigs ())
                {
                    if (isBuyingMode)
                    {
                        if (!prices.ContainsKey (config.item))
                        {
                            prices[config.item] = config.item.GetItemPrice ();
                        }

                        prices[config.item] *= (1 - config.buyingdiscountPercentage / 100);
                    }
                    else
                    {
                        prices[config.item] = config.item.GetItemPrice () * (1 - sellingDisadvantage / 100);
                    }
                }
            }

            return prices;
        }

        private Dictionary<InventoryItem, int> GetAvailabilities ()
        {
            Dictionary<InventoryItem, int> availabilities = new Dictionary<InventoryItem, int> ();

            {
                foreach (StockItemConfig config in GetAvailableConfigs ())
                {
                    if (isBuyingMode)
                    {
                        if (!availabilities.ContainsKey (config.item))
                        {
                            int sold = 0;
                            stockSold.TryGetValue (config.item, out sold);
                            availabilities[config.item] = -sold;
                        }
                        availabilities[config.item] += config.initialStock;
                    }
                    else
                    {
                        availabilities[config.item] = CountItemsInInventory (config.item);
                    }

                }
            }

            return availabilities;
        }

        private IEnumerable<StockItemConfig> GetAvailableConfigs ()
        {
            int shopperLevel = GetShopperLevel ();

            foreach (var config in stockConfig)
            {
                if (config.levelToUnlock > shopperLevel) continue;
                yield return config;

            }
        }

        private void BuyItem (Inventory shopperInventory, Purse shopperPurse, ShopItem shopItem, InventoryItem item, float price)
        {
            if (shopperPurse.GetBalance () < price) return;

            bool success = shopperInventory.AddToFirstEmptySlot (item, 1);
            bool hasStock = shopItem.GetAvailability () > 0;

            if (success && hasStock)
            {
                AddToTransaction (item, -1);
                if (!stockSold.ContainsKey (item))
                {
                    stockSold[item] = 0;
                }
                stockSold[item]++;
                shopperPurse.UpdateBalance (-price);
            }
        }

        private void SellItem (Inventory shopperInventory, Purse shopperPurse, ShopItem shopItem, InventoryItem item, float price)
        {
            int slot = FindFirstItemSlot (shopperInventory, item);
            if (slot == -1) return;

            AddToTransaction (item, -1);
            shopperInventory.RemoveFromSlot (slot, 1);
            if (!stockSold.ContainsKey (item))
            {
                stockSold[item] = 0;
            }
            stockSold[item]--;
            shopperPurse.UpdateBalance (price);
        }

        private int FindFirstItemSlot (Inventory shopperInventory, InventoryItem item)
        {
            for (var i = 0; i < shopperInventory.GetSize (); i++)
            {
                if (item == shopperInventory.GetItemInSlot (i))
                {
                    return i;
                }
            }

            return -1;
        }

        private int GetShopperLevel ()
        {
            BaseStats stats = currentShopper.GetComponent<BaseStats> ();
            if (stats == null) return 0;

            return stats.GetLevel ();
        }

        public object CaptureState ()
        {
            Dictionary<string, int> captureObject = new Dictionary<string, int>();
            foreach (var pair in stockSold)
            {
                captureObject[pair.Key.GetItemID()] = pair.Value;
            }

            return captureObject;
        }

        public void RestoreState (object state)
        {
            Dictionary<string, int> saveObject = (Dictionary<string, int>) state;
            stockSold.Clear();
            foreach (var pair in saveObject)
            {
                stockSold[InventoryItem.GetFromID(pair.Key)] = pair.Value;
            }
        }
    }

}