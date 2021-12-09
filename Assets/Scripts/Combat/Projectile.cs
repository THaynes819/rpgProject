using RPG.Pools;
using RPG.Core;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float projectileSpeed = 10f;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] bool isHoming = false;
        [SerializeField] float destroyDelay = 0.1f;
        [SerializeField] GameObject[] destroyOnHit;
        [SerializeField] UnityEvent hitEnemyEvent;

        Health target = null;
        Vector3 targetPoint;
        GameObject instigator = null;
        float damage = 0f;
        DestroyAfterEffect destroyEffect = null;

        private void Start ()
        {
            transform.LookAt (GetAimLocation ());
        }

        void Update ()
        {
            if (target != null && isHoming && !target.IsDead ())
            {
                transform.LookAt (GetAimLocation ());
            }
            transform.Translate (Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetTarget (Health target, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, target);
        }

        public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, null, targetPoint);
        }

        public void SetTarget (GameObject instigator, float damage, Health target = null, Vector3 targetPoint = default)
        {
            this.target = target;
            this.targetPoint = targetPoint;
            this.damage = damage;
            this.instigator = instigator;

            Destroy (gameObject, maxLifeTime);
        }


        private Vector3 GetAimLocation ()
        {
            if (target == null)
            {
                return targetPoint;
            }

            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider> ();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter (Collider other)
        {
            //Debug.Log("A collider was hit");
            Health health = other.GetComponent<Health>();

            if (target != null && health != target) // do not explode if the target exists and is not dead 
            {
                return;
            } 

            if(target != null && target.IsDead())
            {
                return;
            }
            if (health == null)  // The projectile will explode on buildings and other Non Health colliders
            {
                ErrantProjectile (other);
            }

            if (health != null && health.IsDead())  // do not explode if the collider if it hits a dead object with a health component. 
            {
                ErrantProjectile (other);
            }
            if (other == instigator)  //do not explode if the projuctile hits the instigator/player   .GetComponent<Collider>()
            {
                return;
            }

            if (hitEffect != null && other.gameObject != instigator && other.gameObject.GetComponent<Health>() != null) // After getting through the other If's. Explode if the hit effect is not null and the target is alive
            {
                GameObject newHitEffect = Instantiate (hitEffect, GetAimLocation (), transform.rotation); //Explodes
                hitEnemyEvent.Invoke ();  //TODO this is probably why enemy "Dies twice" Is enemy still dying twice?
                health.TakeDamage (instigator, damage);   // The target's helath takes damage
                DestroyInSteps ();
            }
        }
        private void ErrantProjectile (Collider other)
        {
            if (other.gameObject == instigator)
            {
                return;
            }
            if (hitEffect != null && other.gameObject.GetComponent<Health>() == null) // continue if the other collider is not the player
            {
                GameObject newHitEffect = Instantiate (hitEffect, transform.position, transform.rotation); // Explode
            }
            DestroyInSteps ();            
        }

    private void DestroyInSteps ()
    {
        foreach (GameObject toDestroy in destroyOnHit)
        {
            Destroy (toDestroy);
        }
        Destroy (gameObject, destroyDelay);
    }

    
}
}