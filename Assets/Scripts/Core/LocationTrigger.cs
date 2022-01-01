using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Dialogue;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
        public class LocationTrigger : MonoBehaviour
    {
        [SerializeField] AIConversant aIConversant = null;
        [SerializeField] bool doesWarp = false;
        [SerializeField] bool startsDialogue = false;
        [SerializeField] Transform warpPoint = null;

        GameObject player;
        Dialogue.Dialogue dialogue;



        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            dialogue = aIConversant.GetDialogue();
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject == player)
            {
                player.GetComponent<Mover>().Cancel();
                WarpPlayer();
                StartDialogue();
            }
        }

        private void WarpPlayer()
        {
            if (doesWarp)
            {
                player.GetComponent<NavMeshAgent> ().Warp(warpPoint.position);
            }
        }

        private void StartDialogue()
        {
            if (startsDialogue && aIConversant != null)
            {
                player.GetComponent<PlayerConversant>().StartDialogue (aIConversant, dialogue);
            }
        }

        public void DestroyThis()
        {
            Destroy(this);
        }

    }


}

