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
            currentNode = currentDialogue.GetRootNode();
            onConversationUpdated ();
        }

        public void Quit()
        {
            currentDialogue = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
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
            isChoosing = false;
            NextHandler ();
        }

        public void NextHandler ()
        {
            int playerResponseChoices = currentDialogue.GetPlayerChildren (currentNode).Count ();

            if (playerResponseChoices > 0)
            {
                isChoosing = true;
                onConversationUpdated();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAllChildren (currentNode).ToArray ();
            int randomResponse = UnityEngine.Random.Range (0, children.Count ());
            currentNode = children[randomResponse];
            onConversationUpdated();
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
    }
}