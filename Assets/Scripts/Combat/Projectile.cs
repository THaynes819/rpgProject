using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

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
        Health target = null;
        float damage = 0f;
        DestroyAfterEffect destroyEffect = null;

        private void Start() 
        {            
            transform.LookAt(GetAimLoacation());
        }
        
        void Update()
        {
            if (target == null)
            {
                return;
            }
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLoacation());
            }
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }        

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);     
        }

        private Vector3 GetAimLoacation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }        

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target && !target.IsDead())
            {
                return;
            }
            if (other.GetComponent<Projectile>())
            {
                return;
            }
            ErrantProjectile(other);

            if (hitEffect != null && !target.IsDead())
            {
                GameObject newHitEffect = Instantiate(hitEffect, GetAimLoacation(), transform.rotation);                
            }
            target.TakeDamage(damage);
            DestroyInSteps();
        }

        private void DestroyInSteps()
        {
           foreach (GameObject toDestroy in destroyOnHit)
           {
               Destroy(toDestroy);
           }            
            Destroy(gameObject, destroyDelay);
        }

        private void ErrantProjectile(Collider other)
        {            
            if (other.GetComponent<Health>() != target && target.IsDead()) //Get rid of nested if statement
            {                
                if(other.gameObject != GameObject.FindWithTag("Player"))
                {                    
                    GameObject newHitEffect = Instantiate(hitEffect, transform.position, transform.rotation);
                }    
            }
        }

       


    }
}
