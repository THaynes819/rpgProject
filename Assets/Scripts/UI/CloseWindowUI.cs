using UnityEngine;

    namespace RPG.UI
{
        public class CloseWindowUI : MonoBehaviour 
    {
        
        [SerializeField] UISwitcher switcher = null;
        [SerializeField] GameObject switchToOnClose = null;

        // This Script does nothing more than close the window that is open and open the designated window;
        public void CloseWindow()
        {            
            switcher.SwitchTo(switchToOnClose);      
        }
    }
}