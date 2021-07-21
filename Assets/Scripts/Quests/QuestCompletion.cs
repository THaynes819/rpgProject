using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest quest;
        [SerializeField] string objectiveToComplete;
        [SerializeField] Condition condition;

        string questToCompare;

        bool isObjectiveComplete = false;

        QuestList questList;

        public void CompleteObjective ()
        {
            questList = GameObject.FindGameObjectWithTag ("Player").GetComponent<QuestList> ();
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
            questList = GameObject.FindGameObjectWithTag ("Player").GetComponent<QuestList> ();

            questList.RemoveQuest (quest);
        }
    }

}