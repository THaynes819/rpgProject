using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Inventories;
using RPG.Shops;
using TMPro;
using UnityEngine;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// To be placed on the root of the inventory UI. Handles spawning all the
    /// inventory slot prefabs.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] InventorySlotUI InventoryItemPrefab = null;
        [SerializeField] TextMeshProUGUI purseField;

        // CACHE
        Inventory playerInventory;

        Purse playerPurse;
        Shopper shopper;

        // LIFECYCLE METHODS

        private void Awake ()
        {
            GameObject player = GameObject.FindGameObjectWithTag ("Player");
            playerInventory = Inventory.GetPlayerInventory ();
            playerInventory.inventoryUpdated += Redraw;
            playerPurse = player.GetComponent<Purse>();
            playerPurse.OnPurseUpdated += Redraw;
            shopper = player.GetComponent<Shopper>();
            purseField.text = $"{shopper.GetComponent <Purse>().GetBalance():N0}";
        }

        private void Start ()
        {
            Redraw ();
        }

        // PRIVATE

        private void Redraw ()
        {
            foreach (Transform child in transform)
            {
                Destroy (child.gameObject);
            }

            for (int i = 0; i < playerInventory.GetSize (); i++)
            {
                var itemUI = Instantiate (InventoryItemPrefab, transform);
                itemUI.Setup (playerInventory, i);
            }

            purseField.text = $"{shopper.GetComponent <Purse>().GetBalance():N0}";
        }
    }
}