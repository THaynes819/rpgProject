using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Pools;
using RPG.Core;
using UnityEngine;
using UnityEngine.Events;
using GameDevTV.Inventories;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest quest;
        [SerializeField] string objectiveToComplete;

        [SerializeField] bool hasItemToRemove = false;
        [SerializeField] InventoryItem itemToRemove = null;
        [SerializeField] int amountToRemove = 1;
        [SerializeField] Condition condition;

        string questToCompare;

        bool isObjectiveComplete = false;

        QuestList questList;
        Inventory inventory;

        public void CompleteObjective ()
        {
            var player = GameObject.FindGameObjectWithTag ("Player");
            questList = player.GetComponent<QuestList> ();
            inventory = player.GetComponent<Inventory>();

            if (quest != null)
            {
                questList.CompleteObjective (quest, objectiveToComplete);
                CompleteQuest ();
            }
            if (quest == null)
            {
                return;
            }
        }

        private void CompleteQuest ()
        {
            //Debug.Log("Completing Objective");
            var player = GameObject.FindGameObjectWithTag ("Player");
            questList = player.GetComponent<QuestList> ();
            inventory = player.GetComponent<Inventory>();

            if (hasItemToRemove && itemToRemove != null)
            {
                RemoveQuestItem();
            }

            questList.RemoveQuest (quest);
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

}