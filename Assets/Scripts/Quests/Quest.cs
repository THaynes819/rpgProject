using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu (fileName = "Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject, ISaveable
    {
        [SerializeField] List<Reward> rewards = new List<Reward> ();
        [SerializeField] List<Objective> objectives = new List<Objective> ();

        [System.Serializable]
        public class Reward
        {
            [Min (1)]
            public int number;
            public InventoryItem item;
        }

        [System.Serializable]
        public class Objective 
        {
            [SerializeField] Quest quest;
            QuestList questList;
            QuestStatus status;
            public string reference;
            public string description;
            public bool usesCondition = false;
            public Condition completionCondition;
            [SerializeField] ObjectiveReward[] objectiveRewards;

            [SerializeField] bool isComplete = false;
            public ObjectiveReward[] GetObjectiveRewards(string check)
            {
                if (check == reference)
                {
                    return objectiveRewards;
                }
                return null;
            }
            //public QuestItem itemToRetrieve;
            public void SetObjectiveComplete(string objective, bool value)
            {
                if (reference == objective && value != isComplete)
                {
                    isComplete = value;
                }
            }

            public bool GetIsComplete()
            {
                return isComplete;
            }

            public Quest GetObjectivesQuest(string objective)
            {
                if (objective == reference)
                {
                    return quest;
                }
                return null;
            }

            public Objective GetObjectiveByname(string objective)
            {
                if (objective == reference)
                {
                    return this;
                }
                return null;
            }

            
        }

        [System.Serializable]
        public class ObjectiveReward
        {
            public string objectiveName;
            public InventoryItem item;
            public int number;
        }

        public string GetQuesttitle ()
        {
            return name;
        }

        public int GetObjectiveCount ()
        {
            return objectives.Count;
        }

        public IEnumerable<Objective> GetObjectives ()
        {
            return objectives;
        }

        public bool HasObjective (string objectiveToCheck)
        {
            foreach (Objective objective in objectives)
            {
                if (objective.reference == objectiveToCheck)
                {
                    return true;
                }
            }
            return false;
        }

        public Objective GetObjective(string objectiveToCheck)
        {
            foreach (Objective objective in objectives) 
            {
                if (objective.reference == objectiveToCheck)
                {
                    return objective;
                }
            }
            return null;
        }

        public IEnumerable<Reward> GetRewards ()
        {
            return rewards;
        }


        public static Quest GetByName (string questName)
        {
            foreach (Quest quest in Resources.LoadAll<Quest> (""))
            {
                if (quest.name == questName)
                {
                    return quest;
                }
            }
            return null;
        }

        public object CaptureState()
            {
                List<Objective> saveState = new List<Objective> ();
                foreach (Objective objective in objectives)
                {
                    saveState.Add(objective);
                }
                return saveState;
            }

            public void RestoreState(object state)
            {
                List<Objective> restoreState = state as List<Objective>;
                objectives.Clear();
                foreach (Objective objective in restoreState)
                {
                    objectives.Add(objective);
                }
            }

    }
}