using RPG.Control;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rpg.UI
{
    public class QuitGameUI : MonoBehaviour 
    { 

        SavingWrapper savingWrapper;

        private void Awake() 
        {
            savingWrapper = FindObjectOfType<SavingWrapper>();
        }
        private void OnEnable() 
        {
            
            if (GetComponent<GameFreeze>() != null)
            {
                GetComponent<GameFreeze>().FreezeGame();
            }
            
            if (savingWrapper == null)
            {
                savingWrapper = FindObjectOfType<SavingWrapper>();
            }
        }
        public void SaveAndCloseGame(bool isSaving)
        {
            if (isSaving)
            {
                FindObjectOfType<SavingWrapper>().Save();
            }        
            
            savingWrapper.Save();
            savingWrapper.LoadMenu();
        }

        public void CloseMenu()
        {
            this.gameObject.SetActive(false);
        }

        private void OnDisable() 
        {
            if (GetComponent<GameFreeze>() != null)
            {
            GetComponent<GameFreeze>().ResumeGame(null);
            }
        }
    }
}