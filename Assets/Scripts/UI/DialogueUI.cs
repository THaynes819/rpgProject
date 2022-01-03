using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour, IUICloser
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button quitButton;
        [SerializeField] Button playerResponseButton = null;
        [SerializeField] Transform buttonParent = null;
        [SerializeField] Button responseButtonPrefab = null;
        [SerializeField] string nextText = "Next ->";
        [SerializeField] string goodbyeText = "Goodbye";
        [SerializeField] GameObject aIResponse;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] TextMeshProUGUI conversantName;

        TMP_Text buttonText;
        
        Button goodbyeButtonInstance = null;
        Button nextButtonInstance = null;

        bool isNext = true;
        

        void Start ()
        {
            playerConversant = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerConversant> ();
            
            if (playerConversant == null)
            {
                playerConversant = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerConversant> ();
                playerConversant.onConversationUpdated += UpdateUI;
                buttonText = playerResponseButton.GetComponentInChildren<TMP_Text>();
            }
            if (playerConversant != null)
            {
                playerConversant.onConversationUpdated += UpdateUI;
            }

            quitButton.onClick.AddListener (() => playerConversant.Quit ());

            playerResponseButton.onClick.AddListener (() =>
            {

                if (playerConversant.IsChoosing())
                {
                    BuildChoiceList ();
                }
                else
                {
                    //playerConversant.ResponseHandler ();
                    BuildResponseButton();                    
                }
                
            });            
            
            UpdateUI ();
        }

        public void BuildResponseButton()
        {
            if (playerConversant == null)
            {
                Debug.Log("conversant was null");
                playerConversant = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerConversant> ();
            }

            if (goodbyeButtonInstance != null && goodbyeButtonInstance.GetComponentInChildren<TMP_Text>().text != nextText)
            {
                
                Destroy(goodbyeButtonInstance.gameObject);
                nextButtonInstance = Instantiate(responseButtonPrefab, buttonParent);            
                nextButtonInstance.GetComponentInChildren<TMP_Text>().text = nextText;
                nextButtonInstance.onClick.AddListener (() =>
            {
                if (playerConversant.IsChoosing ())
                {
                    BuildChoiceList ();
                }
                else
                {
                    playerConversant.ResponseHandler ();
                }
                
            });
            }
            if (playerConversant != null)
            {
                playerConversant.ResponseHandler ();
            }
            //playerConversant.ResponseHandler ();
        }

        private void BuildChoiceList ()
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy (item.gameObject);
            }

            foreach (DialogueNode choiceNode in playerConversant.GetChoices ())
            {
                GameObject choiceInstance = Instantiate (choicePrefab, choiceRoot);
                TextMeshProUGUI textInstance = choiceInstance.GetComponentInChildren<TextMeshProUGUI> ();
                textInstance.text = choiceNode.GetText ();
                Button button = choiceInstance.GetComponentInChildren<Button> ();
                button.onClick.AddListener (() =>
                {
                    playerConversant.SelectChoice (choiceNode);
                });
            }
        }


        public void Goodbye()
        {
            //Debug.Log("Goodbye Called");
            if (playerResponseButton != null)
            {
                Destroy(playerResponseButton.gameObject);
            }

            if (nextButtonInstance != null)
            {
                Destroy(nextButtonInstance.gameObject);
            }   
            
            goodbyeButtonInstance = Instantiate(responseButtonPrefab, buttonParent);
            
            goodbyeButtonInstance.GetComponentInChildren<TMP_Text>().text = goodbyeText;

            playerResponseButton.onClick.RemoveAllListeners();
            goodbyeButtonInstance.onClick.AddListener (() =>  playerConversant.Quit ()); 
            

        }

        private void OnDisable() 
        {
            if (playerConversant == null)
            {
                playerConversant = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerConversant> ();
            }
            BuildResponseButton();
        }
        


        void UpdateUI ()
        {
            gameObject.SetActive (playerConversant.IsActive ());
            if (!playerConversant.IsActive ()) return;
            conversantName.text = playerConversant.GetCurrentConversantName ();

            aIResponse.SetActive (!playerConversant.IsChoosing ());
            choiceRoot.gameObject.SetActive (playerConversant.IsChoosing ());
            if (playerConversant.IsChoosing ())
            {
                BuildChoiceList ();
            }
            else
            {
                AIText.text = playerConversant.GetText ();
            }
        }

        public void CloseAll()
        {
            playerConversant.Quit();
        }

        public string GetGameObjectName()
        {
            return this.gameObject.name;
        }

        public bool GetIsActive()
        {
            return gameObject.activeSelf;
        }
    }
}