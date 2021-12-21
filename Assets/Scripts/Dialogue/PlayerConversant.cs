using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string playerName = null;

        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        AIConversant currentConversant = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;

        public string GetPlayerName ()
        {
            if (playerName == null)
            {
                playerName = "Player";
            }
            return playerName;
        }

        public void StartDialogue (AIConversant newConversant, Dialogue newDialogue)
        {
            currentConversant = newConversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode ();
            TriggerEnterAction ();
            onConversationUpdated ();
        }

        public void Quit ()
        {
            currentDialogue = null;
            TriggerExitAction ();
            currentConversant = null;
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
            return FilterOnCondition (currentDialogue.GetPlayerChildren (currentNode));
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
            int playerResponseChoices = FilterOnCondition (currentDialogue.GetPlayerChildren (currentNode)).Count ();

            if (playerResponseChoices > 0)
            {
                isChoosing = true;
                TriggerExitAction ();
                onConversationUpdated ();
                return;
            }

            DialogueNode[] children = FilterOnCondition (currentDialogue.GetAllChildren (currentNode)).ToArray ();

            int randomResponse = 0;

            if (children.Count () > 1)
            {
                randomResponse = UnityEngine.Random.Range (0, children.Count ());
            }
            if (children.Count() == 0)
            {
                Debug.Log("Children Count error occured. child Count is " + children.Count());
                return;
            }
            if (children.Count() == 1)
            {
                randomResponse = 0;
            }
            TriggerExitAction ();
            currentNode = children[randomResponse];
            TriggerEnterAction ();
            onConversationUpdated ();
        }

        public string GetCurrentConversantName ()
        {
            if (isChoosing)
            {
                return playerName;
            }
            else
            {
                return currentConversant.GetNPCName ();
            }
        }

        public bool HasNext ()
        {
            return FilterOnCondition (currentDialogue.GetAllChildren (currentNode)).Count () > 0;
        }

        private IEnumerable<DialogueNode> FilterOnCondition (IEnumerable<DialogueNode> inputNode)
        {
            foreach (var node in inputNode)
            {
                if (node.CheckCondition (GetEvaluators ()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators ()
        {
            return GetComponents<IPredicateEvaluator> ();
        }

        private void TriggerEnterAction ()
        {
            if (currentNode != null)
            {
                TriggerAction (currentNode.GetOnEnterAction ());
            }
        }

        private void TriggerExitAction ()
        {
            if (currentNode != null)
            {
                TriggerAction (currentNode.GetOnExitAction ());
            }              
        }

        private void TriggerAction (string action)
        {
            if (action == "") return;

            foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger> ())
            {
                if (trigger != null)
                {
                    trigger.Trigger (action);
                }
            }
        }
    }
}