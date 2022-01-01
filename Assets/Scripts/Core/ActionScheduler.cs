using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;                   
            if (currentAction != null && action.GetDoesCancel())
            {
                currentAction.Cancel();
            }
            if (action == null)
            {
                CancelCurrentAction();
            }
            currentAction = action;            
        }
    

        public void CancelCurrentAction()
        {
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
        }

    }

}