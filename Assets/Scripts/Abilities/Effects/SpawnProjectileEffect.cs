using System;
using System.Collections;
using RPG.Pools;
using RPG.Combat;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = "RPG/Abilities/Effect/Spawn Projectile", order = 0)]
    public class SpawnProjectileEffect : EffectStrategy
    {

        [SerializeField] Projectile projectileToSpawn;
        [SerializeField] float damage;
        [SerializeField] bool isRightHand = true;
        [SerializeField] bool useTargetPoint = true;
        public override void StartEffect(AbilityData data, Action finished)
        {
            
            Fighter fighter = data.GetUser().GetComponent<Fighter>();
            Vector3 spawnPosition = fighter.GetHandTransform(isRightHand).position;
            if (useTargetPoint)
            {
                SpawnTargetPointProjectile(data, spawnPosition);
            }
            else
            {
                SpawnMultipleProjectiles(data, spawnPosition);
            }
            

            finished();
        }

        private void SpawnTargetPointProjectile(AbilityData data, Vector3 spawnPosition)
        {
            Projectile projectile = Instantiate(projectileToSpawn);                    
                    projectile.transform.position = spawnPosition;
                    projectile.SetTarget(data.GetTargetedPoint(), data.GetUser(), damage);
        }

        private void SpawnMultipleProjectiles(AbilityData data, Vector3 spawnPosition)
            {
                foreach (var target in data.GetTargets())
            {
                Health health = target.GetComponent<Health>();                
                if (health)
                {
                    Projectile projectile = Instantiate(projectileToSpawn);                    
                    projectile.transform.position = spawnPosition;
                    projectile.SetTarget(health, data.GetUser(), damage);
                }
                else
                {
                    Debug.Log("No Enemies Found");
                    return;
                }
            }
            
        }


    }
}