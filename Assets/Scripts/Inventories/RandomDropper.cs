using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        // CONFIG DATA
        [Tooltip ("How far the pickups will sacatter when dropped")]
        [SerializeField] float scatterDistance = 1;
        [SerializeField] DropLibrary[] dropLibraries;
        [SerializeField] int numberOfDrops = 2;

        // Constants
        const int ATTEMPTS = 30;

        public void RandomDrop ()
        {
            var baseStats = GetComponent<BaseStats>();

            foreach (var library in dropLibraries)
            {
                var drops = library.GetRandomDrops(baseStats.GetLevel());
                foreach (var drop in drops)
                {
                    DropItem(drop.item , drop.number);
                }
            }
            
        }


        protected override Vector3 GetDropLocation ()
        {
            // It may take multiple attampts to find a valid drop location
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition (randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position;
        }
    }
}