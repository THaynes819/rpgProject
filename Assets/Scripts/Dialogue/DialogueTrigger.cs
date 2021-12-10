using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] string action = null;
        [SerializeField] string actionTwo = null;
        [SerializeField] UnityEvent onTrigger;

        [SerializeField] UnityEvent onSecondTrigger;

        public void Trigger(string actionTrigger)
        {
            if (actionTrigger == action && action != null)
            {
                onTrigger.Invoke();
            }

            if (actionTrigger == actionTwo && actionTwo != null)
            {
                onSecondTrigger.Invoke();
            }
        }
    }

}