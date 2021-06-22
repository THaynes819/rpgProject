using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveIncompletePrefab;

        public void Setup (QuestStatus status)
        {
            Quest quest = status.GetQuest ();
            title.text = quest.GetQuesttitle ();
            objectiveContainer.DetachChildren ();

            for (var i = 0; i < quest.GetObjectiveCount (); i++)
            {
                GameObject prefab = objectiveIncompletePrefab;
                if (status.isObjectiveComplete(quest.GetObjectives()[i]))
                {
                    prefab = objectivePrefab;
                }

                GameObject objectiveInstance = Instantiate (prefab, objectiveContainer);

                objectiveInstance.GetComponentInChildren<TextMeshProUGUI> ().text = quest.GetObjectives () [i];


            }
        }
    }

}