using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] TriggerGroup[] triggerPairs;

        public void Trigger(string actionTrigger)
        {
            
            foreach (TriggerGroup trigger in triggerPairs)
            {
                if (actionTrigger == trigger.GetAction() && trigger.GetAction() != null)
                {
                    trigger.TriggerPairedEvent();
                }                
            }
        }

        [System.Serializable]
        public class TriggerGroup
        {
            [Tooltip("Quest can be null if it is not a quest reward related trigger")]
            [SerializeField] Quest quest;

            [Tooltip("Must be true if it's a quest reward")]
            [SerializeField] bool isReward;

            [Tooltip("action should be the Objective name and It also must match the Dialogue Trigger if the group is a quest. ")]
            [SerializeField] string action;

            [Tooltip("Trigger is not necessary if it's a quest")]
            [SerializeField] UnityEvent pairedTrigger;

            public string GetAction()
            {
                return action;
            }

            public void TriggerPairedEvent()
            {
                if (pairedTrigger == null)
                {
                    return;
                } 

                // if (quest != null && isReward)
                // {
                //     GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>().CompleteObjective(quest, action);
                //     return;
                // }
                //Debug.Log("Triggering " + pairedTrigger.ToString());  
                
                pairedTrigger.Invoke();
            }
        }
    }

}