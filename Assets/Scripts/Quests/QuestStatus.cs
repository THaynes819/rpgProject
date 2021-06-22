using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [System.Serializable]

    public class QuestStatus
    {
        [SerializeField] Quest quest;
        [SerializeField] List<string> completedObjectives;

        int completedCount;

        public Quest GetQuest()
        {
            return quest;
        }

        public int GetCompletedCount()
        {
            return completedObjectives.Count;
        }

        public List<string> GetCompletedObjectives()
        {
            return completedObjectives;
        }

        public bool isObjectiveComplete(string objective)
        {
            return completedObjectives.Contains(objective);
        }
    }
}