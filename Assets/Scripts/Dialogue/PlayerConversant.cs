using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;

        public void StartDialogue (Dialogue newDialogue)
        {
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode ();
            TriggerEnterAction ();
            onConversationUpdated ();
        }

        public void Quit ()
        {
            currentDialogue = null;
            TriggerExitAction ();
            currentNode = null;
            isChoosing = false;
            onConversationUpdated ();
        }

        public bool IsActive ()
        {
            return currentDialogue != null;
        }

        public bool IsChoosing ()
        {
            return isChoosing;
        }

        public string GetText ()
        {
            if (currentNode == null)
            {
                return "I have nothing to say";
            }

            return currentNode.GetText ();
        }

        public IEnumerable<DialogueNode> GetChoices ()
        {
            return currentDialogue.GetPlayerChildren (currentNode);
        }

        public void SelectChoice (DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction ();
            isChoosing = false;
            NextHandler ();
        }

        public void NextHandler ()
        {
            int playerResponseChoices = currentDialogue.GetPlayerChildren (currentNode).Count ();

            if (playerResponseChoices > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated ();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAllChildren (currentNode).ToArray ();
            int randomResponse = UnityEngine.Random.Range (0, children.Count ());

            TriggerExitAction ();
            currentNode = children[randomResponse];
            TriggerEnterAction ();
            onConversationUpdated ();
        }

        public bool HasNext ()
        {
            if (currentNode != null)
            {
                return currentNode.GetChildren ().Count > 0;
            }
            else
            {
                return false;
            }
        }

        private void TriggerEnterAction ()
        {
            if (currentNode != null && currentNode.GetOnEnterAction () != "")
            {
                Debug.Log (currentNode.GetOnEnterAction ());
            }
        }

        private void TriggerExitAction ()
        {
            if (currentNode != null && currentNode.GetOnExitAction () != "")
            {
                Debug.Log (currentNode.GetOnExitAction ());
            }
        }
    }
}