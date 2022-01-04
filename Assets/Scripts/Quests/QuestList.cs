// Cristian Pop - https://boxophobic.com/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using RPG.Core;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        [SerializeField] float questRemovalDelay = 5f;
        [SerializeField] AudioSource completedSound = null;
        List<QuestStatus> statuses = new List<QuestStatus> ();
        List<QuestStatus> completedStatuses = new List<QuestStatus> ();
        List<Quest> completedQuest = new List<Quest> ();
        List<Quest> removalQuests = new List<Quest> ();
        public event Action OnListUpdated;
        public event Action OnAddQuest;
        public event Action OnQuestUpdated;
        public event Action OnQuestCompleted;

        public void AddQuest (Quest quest)
        {
            if (HasQuest (quest)) return;

            // add a visual que to show the quest log changed

            QuestStatus newStatus = new QuestStatus (quest);
            statuses.Add (newStatus);

            if (OnListUpdated != null)
            {
                OnListUpdated ();
            }

            if (OnAddQuest != null)
            {
                OnAddQuest();
            }
        }

        public List<Quest> GetRemovalQuests()
        {
            return removalQuests;
        }

        // QuestCompletion calls this function so the quest list knows what quests are done but not removed.
        public void QueuQuestRemoval(Quest quest)
        {
            if (!removalQuests.Contains(quest) )
            {
                removalQuests.Add(quest); 
            }
        }

        public void RemoveCompleted(Quest quest)
        {
            if (removalQuests.Contains(quest))
            {
                StartCoroutine(DelayedRemoveQuest(quest));
                removalQuests.Remove(quest);
            }
        }

        public void AlertRemoveQuest(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (quest == status.GetQuest())
                {
                    Debug.Log("Started Coroutine, Removing Quest from Removal List");
                    StartCoroutine(DelayedRemoveQuest(quest));
                }
            }
            // List<Quest> tempQuests = new List<Quest>();
            // tempQuests = removalQuests;

            // if (tempQuests.Contains(quest))
            // {
            //     StartCoroutine(DelayedRemoveQuest(quest));
            //     Debug.Log("Started Coroutine, Removing Quest from Removal List");
            //     //removalQuests.Remove(quest);
            // }
        }

        IEnumerator DelayedRemoveQuest(Quest quest)
        {
            yield return new WaitForSeconds(questRemovalDelay);
            RemoveQuest(quest);
        }

        // Delay this so that it shows completed on the UI for a short time. Interface or observer?
        public void RemoveQuest (Quest quest)
        {

            // add a visual que to show the quest log changed

            if (quest == null)
            {
                return;
            }
            if (!HasQuest (quest))
            {
                return;
            }
            if (statuses != null)
            {
                foreach (QuestStatus status in statuses)
                {
                    if (status.isQuestComplete (status.GetQuest()))
                    {
                        Debug.Log ("All objectives Complete");
                        completedStatuses.Add (status);
                    }
                }
                foreach (QuestStatus completedStatus in completedStatuses)
                {
                    if (statuses.Contains (completedStatus))
                    {
                        statuses.Remove (completedStatus);
                    }
                    if (OnListUpdated != null)
                    {
                        OnListUpdated ();
                    }
                    
                }
            }
        }

        public bool HasQuest (Quest quest)
        {
            return GetActiveQuestStatus (quest) != null;
        }

        public void CompleteObjective (Quest quest, string objective)
        {
            if (quest != null && objective != null)
            {
                QuestStatus status = GetActiveQuestStatus (quest);

                if (status == null) return;

                status.CompleteObjective (status, objective);

                if (status.isQuestComplete (status.GetQuest()))
                {
                    if (OnQuestCompleted != null)
                    {                        
                        OnQuestCompleted();
                    }

                    completedSound.Play();
                    GiveReward (quest);
                }

                if (OnListUpdated != null)
                {
                    OnListUpdated ();
                }
                if (!status.isQuestComplete(status.GetQuest()))
                {
                    if (OnQuestUpdated != null)
                    {
                        Debug.Log("Quest Updated");
                        OnQuestUpdated();
                    }
                }
                
            }

            foreach (QuestStatus status in completedStatuses)
            {
                if (!completedQuest.Contains (status.GetQuest ()) && status.isQuestComplete (status.GetQuest()))
                {
                    completedQuest.Add (status.GetQuest ());
                }
            }
        }

        public IEnumerable<QuestStatus> GetStatuses ()
        {
            return statuses;
        }

        public QuestStatus GetActiveQuestStatus (Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.GetQuest () == quest)
                {
                    return status;
                }
            }
            return null;
        }

        public bool? Evaluate (Predicates predicate, string[] parameters)
        {

            if (predicate == Predicates.HasQuest)
            {
                return HasQuest (Quest.GetByName (parameters[0]));
            }

            if (predicate == Predicates.QuestCompleted && parameters.Length > 0)
            {
                Quest questToCheck = Quest.GetByName (parameters[0]);
                // QuestStatus statusToTest = GetQuestStatus(questToCheck);
                // Debug.Log("Status to test is " + statusToTest);
                if (GetActiveQuestStatus(questToCheck) != null)
                {
                    return GetActiveQuestStatus(questToCheck).isQuestComplete(questToCheck);
                }
                if (GetActiveQuestStatus(questToCheck) == null)
                {
                    return false;
                }
            }

            if (predicate == Predicates.QuestCompleted && parameters.Length == 0)
            {
                Debug.Log ("Parameters was zero...Need to fix this");
                return false;
            }

            return null;
        }

        private void GiveReward (Quest quest)
        {

            foreach (var reward in quest.GetRewards ())
            {
                bool success = GetComponent<Inventory> ().AddToFirstEmptySlot (reward.item, reward.number);
                if (!success)
                {
                    GetComponent<ItemDropper> ().DropItem (reward.item, reward.number);
                }
            }
        }

        public object CaptureState ()
        {
            List<object> state = new List<object> ();
            foreach (QuestStatus status in statuses)
            {
                state.Add (status.CaptureState ());
            }
            return state;
        }

        public void RestoreState (object state)
        {
            List<object> restoreState = state as List<object>;
            if (restoreState == null) return;

            statuses.Clear ();
            foreach (object objectState in restoreState)
            {
                statuses.Add (new QuestStatus (objectState));
            }
        }

        
    }
}