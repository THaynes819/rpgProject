using System.Collections;
using System.Collections.Generic;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using RPG.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// The UI slot for the player action bar.
    /// </summary>
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;
        [SerializeField] Image cooldownOverlay = null;
        // CACHE
        ActionStore actionStore;
        CooldownStore cooldownStore;

        // LIFECYCLE METHODS
        private void Awake ()
        {
            GameObject player = GameObject.FindGameObjectWithTag ("Player");
            actionStore = player.GetComponent<ActionStore> ();
            actionStore.storeUpdated += UpdateIcon;
            cooldownStore = player.GetComponent<CooldownStore>();

        }

        void Update() 
        {
            cooldownOverlay.fillAmount = cooldownStore.GetFractionRemaining(GetItem());
        }

        // PUBLIC

        public void AddItems (InventoryItem item, int number)
        {
            actionStore.AddAction (item, index, number);
        }

        public InventoryItem GetItem ()
        {
            return actionStore.GetAction (index);
        }

        public int GetNumber ()
        {
            return actionStore.GetNumber (index);
        }

        public int MaxAcceptable (InventoryItem item)
        {
            return actionStore.MaxAcceptable (item, index);
        }

        public void RemoveItems (int number)
        {
            Debug.Log("Remove Items Called on ActionSlotUI");
            actionStore.RemoveItems (index, number);
        }

        // PRIVATE

        void UpdateIcon ()
        {
            if (icon != null)
            {
                icon.SetItem (GetItem (), GetNumber ());
            }
        }

    }
}