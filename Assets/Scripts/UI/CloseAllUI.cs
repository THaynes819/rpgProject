using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.UI;
using UnityEngine;

namespace RPG.UI
{
    public class CloseAllUI : MonoBehaviour
    {
    [SerializeField] KeyCode closeKey = KeyCode.Escape;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] float extraPauseDelay = 0.1f;
    IPauseEnabler pauseHider;
    IUICloser[] panels;

        private void OnEnable() 
        {
            pauseHider = pauseMenu.GetComponent<IPauseEnabler>();
            panels = GetComponentsInChildren<IUICloser>();
        }
        void Update()
        {
            if (Input.GetKeyDown(closeKey))
            {
                foreach (var panel in panels) 
                {
                    if (pauseHider.GetPauseAvailability() && panel.GetIsActive())
                    {
                        panel.CloseAll(); //Closes all non Pause Windows and dissalows Pause for a moment
                        StartCoroutine(DisallowPause());
                    }
                    // closes panels open panels that are not the pause menu
                    if (pauseHider.GetPauseAvailability())
                    {
                        panel.CloseAll();                        
                        pauseHider.ClosePauseNow();
                    }
                }
            }
        }

        

        //Dissallows pause for one frame so the pause screen doeswn't pop up when you close all with escape May be unecesarry
        IEnumerator DisallowPause()
        { 
            pauseHider.TogglePauseAvailability(false);            
            yield return new WaitForEndOfFrame();
            StartCoroutine(DelayPauseLonger());     
        }

        IEnumerator DelayPauseLonger()
        {
            yield return new WaitForSeconds(extraPauseDelay);
            pauseHider.TogglePauseAvailability(true);
        }
    }
}



