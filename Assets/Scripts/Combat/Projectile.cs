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
        [SerializeField] float projectileDistance = 40f;
        [SerializeField] bool isHoming = false;
        Health target = null;
        float damage = 0f;

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
            if (other.GetComponent<Health>() != target || target.IsDead())
            {                
                return;
            }
            target.TakeDamage(damage); 
            Destroy(gameObject);
        }
    }
}
