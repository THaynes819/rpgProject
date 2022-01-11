using System;
using System.Collections;
using System.Collections.Generic;
using RPG.UI;
using UnityEngine;

namespace GameDevTV.UI
{
    public class ShowHideUI : MonoBehaviour, IPauseEnabler, IUICloser
    {
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject uiContainer = null;
        [SerializeField] bool isPauseMenu = false;
        [SerializeField] CloseAllUI closeAll = null;
        [SerializeField] float pauseDelay = 0.1f;
        IUICloser[] closeAllUIs;
        bool isPauseEnabled = true;

        void Start ()
        {
            uiContainer.SetActive (false);
            closeAllUIs = closeAll.gameObject.GetComponentsInChildren<IUICloser>();          
        }

        void Update ()
        {
            if (Input.GetKeyDown (toggleKey))
            {
                Toggle ();
            }
        }

        public void Toggle ()
        {
            if (isPauseMenu)
            {
                StartCoroutine(DelayedPauseToggle());
            }
            else
            {
                uiContainer.SetActive (!uiContainer.activeSelf);
            }
        }

        IEnumerator DelayedPauseToggle()
        {            
            if (uiContainer.activeSelf == false) // If Game is not paused
            {
                foreach (var ui in closeAllUIs)
                {
                    ui.CloseAll();
                }
                
                yield return new WaitForEndOfFrame();
                if (!isPauseEnabled) // while closeAll has been used recently, Pause Menu is disabled a moment
                {
                    uiContainer.SetActive(false);
                }
                else
                {
                    uiContainer.SetActive (true); // Extra Delay exists in CloseAll.
                }
            }
            else
            {
                ClosePauseNow(); // Unpause if the game is pause
            }
        }            


        public void CloseAll()
        {
            if (uiContainer.activeSelf == true && !isPauseMenu)
            {
                uiContainer.SetActive(false);
            }
        }

        public string GetGameObjectName()
        {
            return this.gameObject.name;
        }

        public bool GetIsActive()
        {
            return uiContainer.activeSelf;
        }


        public void TogglePauseAvailability(bool value)
        {
            isPauseEnabled = value;
        }

        public bool GetPauseAvailability()
        {
            return isPauseEnabled;
        }

        public void ClosePauseNow()
        {
            if (isPauseMenu && uiContainer.activeSelf == true)
            {
                uiContainer.SetActive(false);
                StartCoroutine(DisablePause());
            }
        }

        IEnumerator DisablePause() //This short delay is so pause doen't immediately open itself upon closing
        {
            isPauseEnabled = false;
            yield return new WaitForSeconds(pauseDelay);
            isPauseEnabled = true;
        }
    }
}