using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] TriggerPair[] triggerPairs;

        public void Trigger(string actionTrigger)
        {
            
            foreach (TriggerPair trigger in triggerPairs)
            {
                if (actionTrigger == trigger.GetAction() && trigger.GetAction() != null)
                {
                    trigger.TriggerPairedEvent();
                }                
            }
        }

        [System.Serializable]
        public class TriggerPair
        {
            [SerializeField] string action;
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
                pairedTrigger.Invoke();               
            }
        }
    }

}