using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour, IUICloser
    {
        [SerializeField] TextMeshProUGUI shopNameText;
        [SerializeField] Button quitButton;
        [SerializeField] Transform listRoot;
        [SerializeField] RowUI rowPrefab;
        [SerializeField] TextMeshProUGUI quantityField;
        [SerializeField] TextMeshProUGUI totalField;
        [SerializeField] TextMeshProUGUI purseField;
        [SerializeField] Button confirmButton;
        [SerializeField] Button modeButton;
        [SerializeField] TextMeshProUGUI modeButtonText;
        [SerializeField] GameObject filterParent;

        Shopper shopper = null;
        Shop currentShop = null;

        void Start ()
        {
            shopper = GameObject.FindGameObjectWithTag ("Player").GetComponent<Shopper> ();
            gameObject.SetActive (currentShop != null);

            if (shopper != null)
            {
                shopper.activeShopChanged += ShopChanged;

                confirmButton.onClick.AddListener (ConfirmTransaction);
                modeButton.onClick.AddListener (SwitchMode);
                quitButton.onClick.AddListener (() => shopper.Quit ());

                ShopChanged ();
            }
            else
            {
                Debug.Log ("Error: Shopper is null");
                return;
            }

        }

        private void ShopChanged ()
        {
            if (currentShop != null)
            {
                currentShop.OnChange -= RefreshUI;
            }

            currentShop = shopper.GetActiveShop ();
            if (currentShop != null)
            {
                shopNameText.text = shopper.GetActiveShop ().GetShopName ();
            }

            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.SetShop(currentShop);
            }

            gameObject.SetActive (currentShop != null);

            if (currentShop == null) return;

            currentShop.OnChange += RefreshUI;

            RefreshUI ();
        }

        private void RefreshUI ()
        {
            foreach (Transform child in listRoot)
            {
                Destroy (child.gameObject);
            }

            foreach (ShopItem shopItem in currentShop.GetFilteredItems ())
            {
                RowUI row = Instantiate<RowUI> (rowPrefab, listRoot);
                row.Setup (currentShop, shopItem);
            }

            purseField.text = $"{shopper.GetComponent <Purse>().GetBalance():N0}";
            totalField.text = $"{currentShop.GetTransactionTotal():N0}";

            confirmButton.interactable = currentShop.CanTransact ();

            if (!currentShop.HasSuccientFunds ())
            {
                totalField.color = Color.red;
            }
            if (currentShop.HasSuccientFunds ())
            {
                totalField.color = Color.white;
            }

            TextMeshProUGUI confirmText = confirmButton.GetComponentInChildren<TextMeshProUGUI> ();

            if (currentShop.IsBuyingMode ())
            {
                modeButtonText.text = "Sell";
                confirmText.text = "Buy";
            }
            else
            {
                modeButtonText.text = "Shop";
                confirmText.text = "Sell";
            }

            foreach (FilterButtonUI button in filterParent.GetComponentsInChildren<FilterButtonUI>())
            {
                button.refreshUI();
            }
        }

        public void Close ()
        {
            shopper.SetActiveShop (null);
        }

        public void ConfirmTransaction ()
        {
            currentShop.ConfirmTransaction ();
        }

        public void SwitchMode ()
        {
            currentShop.SelectMode (!currentShop.IsBuyingMode ());
        }

        public void CloseAll()
        {
            shopper.Quit ();
            //shopper.SetActiveShop (null);
        }

        public string GetGameObjectName()
        {
            return this.gameObject.name;
        }

        public bool GetIsActive()
        {
            return shopper.GetActiveShop() != null;
        }
    }
}