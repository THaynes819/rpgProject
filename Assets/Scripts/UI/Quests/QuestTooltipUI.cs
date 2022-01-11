using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RPG.Quests.Quest;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveIncompletePrefab;
        [SerializeField] TextMeshProUGUI rewardText;

        QuestList questList;
        QuestStatus statusToUpdate;

        void Start ()
        {
            questList = GameObject.FindGameObjectWithTag ("Player").GetComponent<QuestList> ();
            questList.OnListUpdated += UpdateUI;
        }

        public void Setup (QuestStatus status)
        {
            statusToUpdate = status;
            Quest quest = status.GetQuest ();
            UpdateUI ();
        }

        void UpdateUI ()
        {
            title.text = statusToUpdate.GetQuest ().GetQuesttitle ();
            if (objectiveContainer != null)
            {
                foreach (Transform item in objectiveContainer)
                {
                    Destroy (item.gameObject);
                }
            }

            foreach (var objective in statusToUpdate.GetQuest ().GetObjectives ())
            {
                GameObject prefab = objectiveIncompletePrefab;
                if (statusToUpdate.isNamedObjectiveComplete (objective.reference))
                {
                    prefab = objectivePrefab;
                }
                else
                {
                    prefab = objectiveIncompletePrefab;
                }
                GameObject objectiveInstance = Instantiate (prefab, objectiveContainer);
                objectiveInstance.GetComponentInChildren<TextMeshProUGUI> ().text = objective.description;
            }

            rewardText.text = GetRewardText (statusToUpdate.GetQuest ());

        }

        private string GetRewardText (Quest quest)
        {
            string rewardString = "";

            foreach (Reward reward in statusToUpdate.GetQuest ().GetRewards ())
            {
                if (rewardString != "")
                {
                    rewardString += ", ";
                }
                if (reward.number > 1)
                {
                    rewardString += " " + reward.number + " " + reward.item.GetDisplayName ();
                }
                if (reward.number == 1)
                {
                    rewardString += " " + reward.item.GetDisplayName ();
                }
            }

            if (rewardString == "")
            {
                rewardString = "No Reward";
            }
            rewardString += ".";
            return rewardString;

        }

    }
}