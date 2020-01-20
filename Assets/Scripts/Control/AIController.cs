using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;

namespace RPG.Control    
 {   
    
    public class AIController : MonoBehaviour
    {

        [SerializeField] float chaseDistance = 5f;

        GameObject player;
        Fighter fighter;
        float currentDistance;

        private void Update()
        {
            player = GameObject.FindWithTag("Player");
            ChaseBehaviour();    
        }

        private void ChaseBehaviour()
        {
            currentDistance = Vector3.Distance(transform.position, player.transform.position);
            if (currentDistance <= chaseDistance)
            {
                GetComponent<Fighter>().Attack(player);
            }
        }

    }
 }