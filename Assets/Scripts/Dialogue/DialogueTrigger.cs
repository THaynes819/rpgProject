using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] string action = null; // Make this a serializeable array to make it less hard coded
        [SerializeField] string actionTwo = null;
        [SerializeField] string actionThree = null;
        [SerializeField] string actionFour = null;

        [SerializeField] UnityEvent onTrigger;

        [SerializeField] UnityEvent onSecondTrigger;
        [SerializeField] UnityEvent onThirdTrigger;
        [SerializeField] UnityEvent onFourthTrigger;

        public void Trigger(string actionTrigger)
        {
            if (actionTrigger == action && action != null)
            {
                //Debug.Log("Triggering " + actionTrigger); //To be removed, but good to know when unseen things are triggered
                onTrigger.Invoke();
            }

            if (actionTrigger == actionTwo && actionTwo != null)
            {
                //Debug.Log("Triggering " + actionTrigger);
                onSecondTrigger.Invoke();
            }

            if (actionTrigger == actionThree && actionThree != null)
            {
                //Debug.Log("Triggering " + actionTrigger);
                onThirdTrigger.Invoke();
            }

            if (actionTrigger == actionFour && actionFour != null)
            {
                //Debug.Log("Triggering " + actionTrigger);
                onFourthTrigger.Invoke();
            }
        }
    }

}