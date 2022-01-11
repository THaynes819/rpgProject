using System;
using System.Collections;
using RPG.Control;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] Button saveButton = null;
        //[SerializeField] Button saveAndQuitButton = null;
        [SerializeField] Button speedUpButton = null;
        [SerializeField] float timeScaleOnSpeedUp = 5;

        [SerializeField] UISwitcher switcher = null;

        [SerializeField] GameObject quitConfirmUI = null;

        bool isFast = false;

        PlayerController playerController;
        private void Awake() 
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        private void OnEnable()
        {
            if (playerController == null)
            {
                playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            }
            PauseGame();
        }

        public void ToggleSpeedUp()
        {
            isFast = !isFast;
        }

        private void PauseGame()
        {
            
            playerController.SetGameCursor (CursorType.UI); 
            playerController.enabled = false;
            Time.timeScale = 0;
        }

        public void Save()
        {
            FindObjectOfType<SavingWrapper>().Save();
        }

        public void SaveAndQuit()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            quitConfirmUI.SetActive(true);
            
        }

        private void OnDisable()
        {
            if (isFast)
            {
                Time.timeScale = timeScaleOnSpeedUp;
            }
            else
            {
                Time.timeScale = 1;
            }            
            playerController.enabled = true;
        }
    }
}