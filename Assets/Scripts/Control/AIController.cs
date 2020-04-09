using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control    
 {   
    
    public class AIController : MonoBehaviour
    {

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float wayPointTolerance = 1f;

        GameObject player;
        Fighter fighter;
        Health health;
        Mover mover;
        float distaceToPlayer;
        int currentWaypointIndex = 0;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        private void Start() 
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover= GetComponent<Mover>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) { return; }
            ChaseBehaviour();    
        }

        private void ChaseBehaviour()
        {                        
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();
                timeSinceLastSawPlayer = 0f;
            }
            else if (!InAttackRangeOfPlayer() && timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPostiion = guardPosition;            

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPostiion = CetCurrentWaypoint();
            }
            
            mover.StartMoveAction(nextPostiion);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, CetCurrentWaypoint());
            return distanceToWaypoint < wayPointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextindex(currentWaypointIndex);
        }

        private Vector3 CetCurrentWaypoint()        
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentACtion();
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
            distaceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distaceToPlayer <= chaseDistance; 
        }

        // Called by Unity
        private void OnDrawGizmosSelected()         
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);            
        }
    }
 }