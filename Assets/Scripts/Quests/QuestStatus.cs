using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Quests
{

    public class QuestStatus
    {
        Quest quest;
        List<string> completedObjectives = new List<string> ();

        int completedCount;
        bool isComplete = false;

        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives = new List<string> ();
        }

        public QuestStatus (Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus (object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            quest = Quest.GetByName (state.questName);
            completedObjectives = state.completedObjectives;
        }

        public Quest GetQuest ()
        {
            return quest;
        }

        public int GetCompletedCount ()
        {
            return completedObjectives.Count;
        }

        public List<string> GetCompletedObjectives ()
        {
            return completedObjectives;
        }

        public bool isObjectiveComplete (string objective)
        {
            if (quest != null)
            {
                if (completedObjectives.Contains (objective))
                {
                    return true;
                }
            }
            return false;
        }

        public void CompleteObjective (QuestStatus status, string objective)
        {
            if (!completedObjectives.Contains (objective) && status.GetQuest ().HasObjective (objective))
            {
                completedObjectives.Add (objective);
            }

            if (objective == null)
            {
                Debug.Log ("The objective passed to A Status was null");
            }

            else
            {
                //Objective already complete or quest not yet started
                return;
            }
        }

        public bool isQuestComplete (Quest questToCheck)
        {
            foreach (var objective in questToCheck.GetObjectives ())
            {
                if (!completedObjectives.Contains (objective.reference))
                {
                    return false;
                }
            }
            return true;
        }

        public object CaptureState ()
        {
            QuestStatusRecord state = new QuestStatusRecord ();
            state.questName = quest.name;
            state.completedObjectives = completedObjectives;
            return state;
        }
    }

}