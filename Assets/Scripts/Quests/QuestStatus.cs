using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Quests
{

    public class QuestStatus : ISaveable
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

        public bool IsComplete()
        {
            foreach (var objective in quest.GetObjectives())
            {
                if (!completedObjectives.Contains(objective.reference))
                {
                    return false;
                }
            }
            return true;
        }

        public bool isNamedObjectiveComplete (string objective)
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
                Quest.Objective objectiveToCheck = status.GetQuest().GetObjective(objective);
                if (objectiveToCheck.GetObjectiveRewards(objective) != null)
                {
                    Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
                    foreach (Quest.ObjectiveReward reward in objectiveToCheck.GetObjectiveRewards(objective))
                    {
                        inventory.AddToFirstEmptySlot(reward.item, reward.number);
                    }
                }
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
            //Debug.Log("IsQuestComplete Called for " + questToCheck);
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
            QuestStatusRecord captureState = new QuestStatusRecord ();
            captureState.questName = quest.name;
            captureState.completedObjectives = completedObjectives;
            return captureState;
        }

        public void RestoreState(object state)
        {
            QuestStatusRecord restoreState = new QuestStatusRecord ();
            restoreState = state as QuestStatusRecord;
            quest.name = restoreState.questName;
            completedObjectives = restoreState.completedObjectives;
        }
    }

}