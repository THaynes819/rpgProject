using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{   
    public class CinematicControlRemover : MonoBehaviour 
    {

        PlayerController playerController;

        private void Start() 
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            playerController = FindObjectOfType<PlayerController>();
        }

        void DisableControl(PlayableDirector playableDirector)
        {
            
            GameObject player = GameObject.FindWithTag("Player");            
            player.GetComponent<ActionScheduler>().CancelCurrentACtion();
            playerController.enabled = false;

        }

        void EnableControl(PlayableDirector playableDirector)
        {
            playerController.enabled = true;
        }
    }
}