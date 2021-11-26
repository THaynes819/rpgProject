using RPG.Attributes;
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

        //Health target = null;
        Vector3 targetPoint;
        GameObject instigator = null;
        float damage = 0f;
        DestroyAfterEffect destroyEffect = null;

        private void Start ()
        {
            Debug.Log("Projectile.cs Started");
            transform.LookAt (GetAimLoacation ());
        }

        void Update ()
        {
            if (target != null && isHoming && !target.IsDead ())
            {
                transform.LookAt (GetAimLoacation ());
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


        private Vector3 GetAimLoacation ()
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
            Health health = other.GetComponent<Health>();
            if (target != null && health != target && !target.IsDead ()) // do not explode if the target exists and is not dead  
            {                                                            // and the other collider entered has a health component
                return;
            }
            if (other.GetComponent<Projectile> ()) // do not explode if the other collider is also a projectile
            {
                return;
            }
            if (other == instigator.GetComponent<Collider>()) //do not explode if the projuctile hits the instigator/player
            {
                return;
            }
            ErrantProjectile (other);

            if (hitEffect != null && !target.IsDead ()) // After getting through the other If's. Explode if the hit effect is not null and the target is alive
            {
                GameObject newHitEffect = Instantiate (hitEffect, GetAimLoacation (), transform.rotation); //Explodes
            }
            hitEnemyEvent.Invoke ();  //TODO this is probably why enemy "Dies twice" Is enemy still dting twice?
            target.TakeDamage (instigator, damage);   // The target takes damage
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

    private void ErrantProjectile (Collider other)
    {
        if (other.GetComponent<Health> () != target && target.IsDead ()) // continue if the other collider is not the target and the target is dead
        {
            if (other.gameObject != GameObject.FindWithTag ("Player")) // continue if the other collider is not the player
            {
                GameObject newHitEffect = Instantiate (hitEffect, transform.position, transform.rotation); // Explode
            }
        }
    }
}
}