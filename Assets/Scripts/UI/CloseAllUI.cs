using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class CloseAllUI : MonoBehaviour
    {
    [SerializeField] KeyCode closeKey = KeyCode.Escape;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] float pauseDisallowedTime = 1f;

    bool closedRecently = false;

        void Update()
        {
            if (Input.GetKeyDown(closeKey))
            {
                foreach (var panel in GetComponentsInChildren<IUICloser>())
                {
                    //this finds the pause panel, Something can be done with this later if necesarry.
                    if (panel.GetGameObjectName() == pauseMenu.name && panel.GetIsActive())
                    {
                        
                    }
                    // closes panels open panels that are not the pause menu
                    if (panel.GetGameObjectName() != pauseMenu.name && panel.GetIsActive())
                    {
                        panel.CloseAll();
                        StartCoroutine(DisallowPause()); 
                    }
                }
            }
        }

        //Dissallows pause for one frame so the pause screen doeswn't pop up when you close all with escape May be unecesarry
        IEnumerator DisallowPause()
        { 
            closedRecently = true;
            yield return new WaitForEndOfFrame();
            closedRecently = false;     
        }

        public bool GetRecentlyClosed()
        {
            return closedRecently;
        }
    }
}



