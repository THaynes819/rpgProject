using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Pools;
using RPG.Core;
using UnityEngine;
using UnityEngine.Events;
using GameDevTV.Inventories;
using GameDevTV.Utils;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [System.Serializable]
        public class ObjectiveCompletion
        {
            public bool hasItemToRemove = false;
            public InventoryItem itemToRemove = null;
            public int amountToRemove = 1;
            public Condition condition;
            public Quest quest;
            public Quest.Objective objective;
        }

        [SerializeField] ObjectiveCompletion[] objectiveCompletions;
        bool isObjectiveComplete = false;

        QuestList questList;
        Inventory inventory;

        // This is called by Unity Events, Not by code
        public void CompleteObjective (String reference)
        {
            
            //Debug.Log("Completing Objective" + reference); //TODO maybe add the complete objective sound here instead?
            
            var player = GameObject.FindGameObjectWithTag ("Player");
            questList = player.GetComponent<QuestList> ();
            inventory = player.GetComponent<Inventory>();
            
            foreach (var completion in objectiveCompletions)
            {
                if (reference == completion.objective.reference)
                {
                    questList.CompleteObjective(completion.quest, completion.objective.reference);
                }
                if (completion.hasItemToRemove && completion.itemToRemove != null)
            {
                RemoveQuestItem(completion);
            }
            }
        }

        private void RemoveQuestItem(ObjectiveCompletion completion)
        {
            if (completion.hasItemToRemove && completion.itemToRemove != null)
            {
                if (inventory.HasItem(completion.itemToRemove))
                {
                    inventory.RemoveItem(completion.itemToRemove, completion.amountToRemove);
                }
            }
            
            
        }
    }

}