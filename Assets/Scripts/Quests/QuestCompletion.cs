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
        [SerializeField] QuestObjectivePair[] objectivePairs;
        [SerializeField] bool hasItemToRemove = false;
        [SerializeField] InventoryItem itemToRemove = null;
        [SerializeField] int amountToRemove = 1;
        [SerializeField] Condition condition;

        string questToCompare;

        bool isObjectiveComplete = false;

        QuestList questList;
        Inventory inventory;

        public void CompleteObjective (QuestObjectivePair objective)
        {
            var player = GameObject.FindGameObjectWithTag ("Player");
            questList = player.GetComponent<QuestList> ();
            inventory = player.GetComponent<Inventory>();
            foreach (var pair in objectivePairs)
            {
                if (pair.GetPairedQuest() == null) return;

                if (objective != null && objective == pair)
                {
                        Quest quest = objective.GetPairedQuest();
                        string objectiveToComplete = objective.GetPairedObjective();
                        questList.CompleteObjective (quest, objectiveToComplete );
                        CompleteQuest (quest);
                }
                
            }
            

            
        }

        private void CompleteQuest (Quest completedQuest)
        {
            //Debug.Log("Completing Objective");
            var player = GameObject.FindGameObjectWithTag ("Player");
            questList = player.GetComponent<QuestList> ();
            inventory = player.GetComponent<Inventory>();

            if (hasItemToRemove && itemToRemove != null)
            {
                RemoveQuestItem();
            }

            questList.QueuQuestRemoval (completedQuest);
        }

        private void RemoveQuestItem()
        {
            if (hasItemToRemove)
            {
                if (inventory.HasItem(itemToRemove))
                {
                    inventory.RemoveItem(itemToRemove, amountToRemove);
                }
            }
            
            
        }
    }

    [System.Serializable]
    public class QuestObjectivePair
    {
        [SerializeField] Quest quest;
        [SerializeField] string objective;

        public Quest GetPairedQuest()
        {
            return quest;
        }

        public string GetPairedObjective()
        {
            return objective;
        }
    }

}