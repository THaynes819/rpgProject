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
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button quitButton;
        [SerializeField] Button nextButton;
        [SerializeField] GameObject aIResponse;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] TextMeshProUGUI conversantName;

        void Start ()
        {
            playerConversant = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerConversant> ();
            playerConversant.onConversationUpdated += UpdateUI;

            quitButton.onClick.AddListener (() => playerConversant.Quit ());
            nextButton.onClick.AddListener (() =>
            {
                if (playerConversant.IsChoosing ())
                {
                    BuildChoiceList ();
                }
                else
                {
                    playerConversant.NextHandler ();
                }
            });
            UpdateUI ();
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
                nextButton.gameObject.SetActive (playerConversant.HasNext ());
            }
        }

    }
}