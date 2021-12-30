using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{   
    public class CinematicControlRemover : MonoBehaviour 
    {

        PlayerController playerController;

        private void Awake() 
        {            
            playerController = FindObjectOfType<PlayerController>();
        }

        
        void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable() 
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        void DisableControl(PlayableDirector playableDirector)
        {
            
            GameObject player = GameObject.FindWithTag("Player");            
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            playerController.enabled = false;

        }

        void EnableControl(PlayableDirector playableDirector)
        {
            playerController.enabled = true;
        }
    }
}