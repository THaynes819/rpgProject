using System;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable, IItemStore
    {
        [SerializeField] float startingBalance = 400f;
        float balance = 0f;

        public event Action OnPurseUpdated;

        private void Awake ()
        {
            balance = startingBalance;
        }

        public float GetBalance ()
        {
            return balance;
        }

        public void UpdateBalance (float amount)
        {
            balance += amount;

            OnPurseUpdated?.Invoke();
        }

        public object CaptureState()
        {
            float saveObject = balance;
            return saveObject;
        }

        public void RestoreState(object state)
        {
            balance = (float) state;
        }

        public int AddItems(InventoryItem item, int number)
        {
            
            if (item is CurrencyItem)
            {
                
                float price = item.GetItemPrice();
                UpdateBalance(number * price);
                
                return number;
            }            
            return 0;
            
        }
    }
}