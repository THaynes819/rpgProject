using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {

    bool alreadyTriggered = false;

    GameObject player;

        private void Start() 
        {
            player = GameObject.FindWithTag("Player");
        }

        private void OnTriggerEnter(Collider other) 
        {            
            if (!alreadyTriggered && other.gameObject == player)
            {                
                GetComponent<PlayableDirector>().Play();
                alreadyTriggered = true;               
            }
            else
            {
                return;
            }    
        }       
    }
}