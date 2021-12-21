using System;
using System.Collections;
using GameDevTV.Utils;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour 
    {
        [SerializeField] float fadeintime = 0.5f;
        [SerializeField] GameObject mainMenuPanel = null;
        [SerializeField] GameObject newGameMenu = null;

        [SerializeField] Button newGameBackButton = null;
        [SerializeField] Button newButton = null;
        [SerializeField] Button saveButton = null;
        [SerializeField] Button loadButton = null;
        [SerializeField] Button settingsButton = null;
        [SerializeField] GameObject settingsPopUp = null;
        [SerializeField] Button settingsCloseButton = null;
        [SerializeField] Button quitButton = null;
        [SerializeField] GameObject quitPopUp = null;

        [SerializeField] Button confirmQuitButton = null;
        [SerializeField] Button doNotQuitButton = null;

        [SerializeField] UISwitcher switcher = null;
        LazyValue<SavingWrapper> savingWrapper;

        private void Awake() 
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        public void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }


        public void SaveGame()
        {
            savingWrapper.value.Save(); // add ability to rename save here?
        }

        public void LoadGame()
        {
            savingWrapper.value.Load();
        }

        public void QuitConfirm()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL(webplayerQuitURL)
#else
            Application.Quit();
#endif
        }
    }
}
