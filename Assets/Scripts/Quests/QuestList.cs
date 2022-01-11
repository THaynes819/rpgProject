// Cristian Pop - https://boxophobic.com/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using GameDevTV.Utils;
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

        private void Update() 
        {
            CompleteObjectivesByPredicates();
        }

        
        public void ClearObjectives(Quest quest)
        {
            //Debug.Log("Clear Objective Called From in game");
            if (statuses.Count <= 0 && completedQuest.Count <= 0)
            {
                Debug.Log("Status and Completed are 0, Calling SetObjectiveComplete");
                foreach (var objective in quest.GetObjectives())
                {
                    objective.SetObjectiveComplete(objective.reference, false);
                }
                
                //Debug.Log("Status should have been set");
                return;
            }

            if (statuses.Count > 0)
            {
                foreach (var status in statuses)
                {
                    if (status.GetQuest() == quest)
                    {
                        foreach (var objective in quest.GetObjectives())
                        {
                            if (!status.isNamedObjectiveComplete(objective.reference))
                            {
                                //Debug.Log("Incomplete Objective found in statuses, it's " + objective.reference);
                                objective.SetObjectiveComplete(objective.reference, false);
                                return;
                            }
                        }
                    }
                }
            }
            Debug.Log("Incompleted no Objectives");
        }

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
                    StartCoroutine(DelayedRemoveQuest(quest));
                }
            }
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

                Quest.Objective completedObjective = quest.GetObjective(objective);
                //Debug.Log("Setting Objective " + objective + " as complete");
                completedObjective.SetObjectiveComplete(objective, true);
                

                if (status.isQuestComplete (status.GetQuest()))
                {
                    if (OnQuestCompleted != null)
                    {                        
                        OnQuestCompleted();
                    }

                    completedSound.Play();
                    GiveReward (quest);
                    QueuQuestRemoval (quest);
                }

                if (OnListUpdated != null)
                {
                    OnListUpdated ();
                }
                if (!status.isQuestComplete(status.GetQuest()))
                {
                    if (OnQuestUpdated != null)
                    {
                        Debug.Log("Quest Updated");   //TODO maybe add a sound here
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

        public bool? Evaluate (Predicates predicate, string[] parameters, RequiredAttribute[] attributes)
        {

            if (parameters.Length > 0)
            {
                    switch (predicate)
                {
                    case Predicates.HasQuest:
                    return HasQuest (Quest.GetByName (parameters[0]));                
                    case Predicates.ObjectiveComplete:
                    return CheckObjectiveComplete(predicate, parameters);
                    case Predicates.QuestCompleted:
                    return CheckQuestComplete(predicate, parameters);
                }
            }
            
            if (predicate == Predicates.QuestCompleted && parameters.Length == 0)
            {
                Debug.Log ("Parameters was zero...Need to fix this");
                return false;
            }
            return null;
        }

        private bool? CheckQuestComplete(Predicates predicate, string[] parameters)
        {
            if (predicate == Predicates.QuestCompleted && parameters.Length > 0 )
            {
                //Debug.Log("Checking Quest Parameter " + parameters[0]);
                Quest quest = Quest.GetByName(parameters[0]);
                QuestStatus status = GetActiveQuestStatus(quest);
                if (status == null)
                {
                    return null;
                }
                //Debug.Log("the Status is " + status + ". The quest is " + quest);
                bool isComplete = status.isQuestComplete(quest);
                return isComplete;
                //return GetActiveQuestStatus(Quest.GetByName(parameters[0])).isQuestComplete(Quest.GetByName(parameters[0]));
            }
            return null;
        }
        private bool? CheckObjectiveComplete(Predicates predicate, string[] parameters)
        {
            if (predicate == Predicates.ObjectiveComplete && parameters.Length > 0)
            {
                foreach (var status in statuses)
                {
                    Quest quest = status.GetQuest();
                    Quest.Objective objective;
                    if (quest.GetObjective(parameters[0]) != null)
                    {
                        objective = quest.GetObjective(parameters[0]);
                        //Debug.Log("Returning" + parameters[0] + " Objective Complete as " + objective.GetIsComplete());
                        return objective.GetIsComplete();
                    }
                }
                
            }
            return null;
        }

        public RPG.Stats.Attribute[] GetRequiredAttributes()
            {
                return null;
            }

        public float GetRequiredValue()
        {
            return 0f;
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

        private void CompleteObjectivesByPredicates()
        {
            if (statuses.Count > 0)
            {
                foreach (var status in statuses)
                {
                    if (status.IsComplete()) continue;
                    Quest quest = status.GetQuest();
                    foreach (var objective in quest.GetObjectives())
                    {
                        if (status.isNamedObjectiveComplete(objective.reference)) continue;
                        if (!objective.usesCondition) continue;
                         if (objective.completionCondition.Check(GetComponents<IPredicateEvaluator>())) // This GetComponent is in an Updated loop. Need to cache, but doesn't use much resource
                        {
                            CompleteObjective(quest, objective.reference);
                        } 
                    } 
                    
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