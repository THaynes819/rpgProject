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

        Health target = null;
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
            if (target == null)
            {
                return;
            }
            if (isHoming && !target.IsDead ())
            {
                transform.LookAt (GetAimLoacation ());
            }
            transform.Translate (Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetTarget (Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy (gameObject, maxLifeTime);
        }


        private Vector3 GetAimLoacation ()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider> ();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter (Collider other)
        {
            if (other.GetComponent<Health> () != target && !target.IsDead ())
            {
                return;
            }
            if (other.GetComponent<Projectile> ())
            {
                return;
            }
            if (other == instigator.GetComponent<Collider>())
            {
                return;
            }
            ErrantProjectile (other);

            if (hitEffect != null && !target.IsDead ())
            {
                GameObject newHitEffect = Instantiate (hitEffect, GetAimLoacation (), transform.rotation);
            }
            hitEnemyEvent.Invoke ();  //TODO this is probably why enemy "Dies twice"
            target.TakeDamage (instigator, damage);
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
        if (other.GetComponent<Health> () != target && target.IsDead ()) //Get rid of nested if statement
        {
            if (other.gameObject != GameObject.FindWithTag ("Player"))
            {
                GameObject newHitEffect = Instantiate (hitEffect, transform.position, transform.rotation);
            }
        }
    }
}
}