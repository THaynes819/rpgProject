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
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0,1)][SerializeField] float patrolSpeedFraction = 0.2f;

        GameObject player;
        Fighter fighter;
        Health health;
        Mover mover;        
        float distaceToPlayer;        
        int currentWaypointIndex = 0;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        public float timeSinceArrivedAtWaypoint = Mathf.Infinity;     

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
            UpdateTimers();
            
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void ChaseBehaviour()
        {                        
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();                
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
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPostiion = CetCurrentWaypoint();
            }
            
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPostiion, patrolSpeedFraction);
            }
            
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
            
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0f;
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