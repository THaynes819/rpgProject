using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.UI
{        public class DisableController : MonoBehaviour
    {

        [SerializeField] float disableWait = 0.1f;

        PlayerController playerController;
        private void Awake() 
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            
        }
        private void OnEnable()
        {      
            playerController.SetGameCursor (CursorType.UI); 
            StartCoroutine(DisablePlayer());   
        }

        //Coroutine is to make sure the cursor changes before the player controller is disabled. This ensured the correct cursor for dialogue and shopping.
        IEnumerator DisablePlayer()
        {
            yield return new WaitForSeconds(disableWait);
            playerController.enabled = false;
        }

        private void OnDisable()
        {
            playerController.enabled = true;
        }
    }
}

