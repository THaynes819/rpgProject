using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {

        [SerializeField] Quest[] quests;

        Quest quest;

        public void GiveQuest (string questName)
        {
            QuestList questList = GameObject.FindGameObjectWithTag ("Player").GetComponent<QuestList> ();
            foreach (Quest givenQuest in quests)
            {
                if (questName == givenQuest.name)
                {
                    questList.AddQuest(givenQuest);
                }
                if (quest == null)
                {
                    quest = givenQuest;
                }
            }
        }

        public Quest GetGiversQuest()
        {
            return quest;
        }

    }
}