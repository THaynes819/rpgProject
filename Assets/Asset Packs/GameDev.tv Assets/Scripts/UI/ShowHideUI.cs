using System;
using System.Collections;
using System.Collections.Generic;
using RPG.UI;
using UnityEngine;

namespace GameDevTV.UI
{
    public class ShowHideUI : MonoBehaviour, IUICloser
    {
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject uiContainer = null;
        [SerializeField] bool isPauseMenu = false;
        [SerializeField] CloseAllUI closeAll = null;

        void Start ()
        {
            uiContainer.SetActive (false);
            if (closeAll != null)
            {
                closeAll = closeAll.GetComponent<CloseAllUI>();
            }
            
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
                if (uiContainer.activeSelf == false)
                {
                    // while closeAll has been used recently, this shouldn't work.
                    if (closeAll.GetRecentlyClosed())
                    {
                        uiContainer.SetActive(false);
                    }
                    else
                    {
                        StartCoroutine(DelayedPause());                        
                    }
                }
                else
                {
                    uiContainer.SetActive (false);
                }
                
            }
            else
            {
                uiContainer.SetActive (!uiContainer.activeSelf);
            }
            
            
            
            
            
        }

        IEnumerator DelayedPause()
        {
            yield return new WaitForEndOfFrame();
            uiContainer.SetActive (true);
        }

        public void CloseAll()
        {
            if (uiContainer.activeSelf == true)
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
    }
}