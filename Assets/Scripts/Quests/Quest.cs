using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu (fileName = "Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject
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
            public string reference;
            public string description;
            //public QuestItem itemToRetrieve;
            public bool hasBossToKill = false;
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

        // public InventoryItem GetItemToRetrieve(Quest questToCheck)
        // {
        //     foreach (var objective in questToCheck.GetObjectives())
        //     {
        //         if (objective.itemToRetrieve != null)
        //         {
        //             return objective.itemToRetrieve;
        //         }
        //     }
        //     return null;
        // }



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
                    Debug.Log("Get By Name returning " + quest.name);
                    return quest;
                }
            }
            return null;
        }

    }
}