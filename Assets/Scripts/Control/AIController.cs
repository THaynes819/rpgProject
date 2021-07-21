using System;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{

    public class AIController : MonoBehaviour
    {

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float wayPointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range (0, 1)][SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float aggroCooldownTime = 5f;
        [SerializeField] float helpMeDistance = 5f;

        GameObject player;
        Fighter fighter;
        Health health;
        Mover mover;
        float distaceToPlayer;
        int currentWaypointIndex = 0;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        public float timeSinceArrivedAtWaypoint = Mathf.Infinity; // public to monitor in the inspector
        public float timeSinceAggravated = Mathf.Infinity; // ditto

        LazyValue<Vector3> guardPosition;

        private void Awake ()
        {
            player = GameObject.FindWithTag ("Player");
            fighter = GetComponent<Fighter> ();
            health = GetComponent<Health> ();
            mover = GetComponent<Mover> ();
            guardPosition = new LazyValue<Vector3> (GetInitialGuardPosition);
        }

        private Vector3 GetInitialGuardPosition ()
        {
            return transform.position;
        }

        private void Start ()
        {
            guardPosition.ForceInit ();
        }

        private void Update ()
        {
            if (health.IsDead ()) { return; }
            GuardBevaviour ();
            UpdateTimers ();

        }

        public void Aggravate ()
        {
            timeSinceAggravated = 0f;
        }

        private void UpdateTimers ()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggravated += Time.deltaTime;
        }

        private void GuardBevaviour ()
        {
            if (IsAggravated () && fighter.CanAttack (player))
            {
                AttackBehaviour ();
            }
            else if (!IsAggravated () && timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour ();
            }
            else
            {
                PatrolBehaviour ();
            }
        }

        private void PatrolBehaviour ()
        {

            Vector3 nextPostiion = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint ())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint ();
                }
                nextPostiion = CetCurrentWaypoint ();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction (nextPostiion, patrolSpeedFraction);
            }
        }

        private bool AtWaypoint ()
        {
            float distanceToWaypoint = Vector3.Distance (transform.position, CetCurrentWaypoint ());
            return distanceToWaypoint < wayPointTolerance;
        }

        private void CycleWaypoint ()
        {
            currentWaypointIndex = patrolPath.GetNextindex (currentWaypointIndex);
        }

        private Vector3 CetCurrentWaypoint ()
        {
            return patrolPath.GetWaypoint (currentWaypointIndex);
        }

        private void SuspicionBehaviour ()
        {
            GetComponent<ActionScheduler> ().CancelCurrentACtion ();
        }

        private void AttackBehaviour ()
        {
            timeSinceLastSawPlayer = 0f;
            fighter.Attack (player);

            AgrravateNearbyEnemies ();
        }

        private void AgrravateNearbyEnemies ()
        {
            RaycastHit[] hits = Physics.SphereCastAll (transform.position, helpMeDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController> ();
                if (ai == null) continue;

                ai.Aggravate ();
            }
        }

        private bool IsAggravated ()
        {
            distaceToPlayer = Vector3.Distance (transform.position, player.transform.position);
            return distaceToPlayer <= chaseDistance || timeSinceAggravated <= aggroCooldownTime;
        }

        private void OnDrawGizmosSelected ()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere (transform.position, chaseDistance);
        }

    }
}