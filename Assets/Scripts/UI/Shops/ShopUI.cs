using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.Inventories;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopNameText;
        [SerializeField] Button quitButton;
        [SerializeField] Transform listRoot;
        [SerializeField] RowUI rowPrefab;
        [SerializeField] TextMeshProUGUI totalField;
        [SerializeField] TextMeshProUGUI purseField;

        Shopper shopper = null;
        Shop currentShop = null;

        void Start ()
        {
            shopper = GameObject.FindGameObjectWithTag ("Player").GetComponent<Shopper> ();
            gameObject.SetActive (currentShop != null);

            if (shopper != null)
            {
                shopper.activeShopChanged += ShopChanged;

                quitButton.onClick.AddListener (() => shopper.Quit ());
                ShopChanged ();
            }
            else
            {
                Debug.Log ("Error: Shopper is null");
                return;
            }

        }

        public void ConfirmTransaction ()
        {
            currentShop.ConfirmTransaction ();
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
        }
    }
}