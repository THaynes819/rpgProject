using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable
    {
        [SerializeField] float startingBalance = 400f;

        float balance = 0f;

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
    }
}