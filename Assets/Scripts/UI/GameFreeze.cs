using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

public class GameFreeze : MonoBehaviour
{
    PlayerController playerController;
    
    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void FreezeGame() 
    {
        Time.timeScale = 0;
        playerController.enabled = false;
    }

    public void ResumeGame(GameObject sendingMenu) 
    {
        if (sendingMenu == null)
        {
            Time.timeScale = 1;
            playerController.enabled = true;
        }

        if (sendingMenu != null && !sendingMenu.activeSelf)
        {
            Time.timeScale = 1;
            playerController.enabled = true;
        }
        if (sendingMenu != null && sendingMenu.activeSelf)
        {
            Time.timeScale = 0;
            playerController.enabled = false;
        }
    }
    
}
