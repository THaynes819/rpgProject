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
        [SerializeField] Button continueButton = null;
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

        private void Start() 
        {
            continueButton.onClick.AddListener(() => ContinueGame());
            newButton.onClick.AddListener(() => CreateGame());
            newGameBackButton.onClick.AddListener(() => CloseSecondaryMenu());
            saveButton.onClick.AddListener(() => SaveGame());
            loadButton.onClick.AddListener(() => LoadGame());
            settingsButton.onClick.AddListener(() => OpenSettings());
            settingsCloseButton.onClick.AddListener(() => CloseSecondaryMenu());
            quitButton.onClick.AddListener(() => QuitGameConfirmation());
            confirmQuitButton.onClick.AddListener(() => QuitConfirm());
            doNotQuitButton.onClick.AddListener(() => CloseSecondaryMenu());            
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        private void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }

        private void CreateGame()
        {
            switcher.SwitchTo(newGameMenu);
        }

        private void SaveGame()
        {
            savingWrapper.value.Save();
        }

        private void LoadGame()
        {
            savingWrapper.value.Load();
        }
        private void OpenSettings()
        {
            switcher.SwitchTo(settingsPopUp);
        }

        private void CloseSecondaryMenu()
        {
            switcher.SwitchTo(mainMenuPanel);
        }
        private void QuitGameConfirmation()
        {
            //Pop up an are you sure you want to quit window
            switcher.SwitchTo(quitPopUp);
        }

        public void QuitConfirm()
        {
            Application.Quit();
        }
    }
}