using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
                
        [SerializeField] float projectileSpeed = 1f;
        Health target = null;
        float damage = 0f;

        void Update()
        {
            if (target == null)
            {
                return;
            }
            transform.LookAt(GetAimLoacation());
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
            if (other.GetComponent<Health>() != target)
            {
                return;
            }
            target.TakeDamage(damage); 
            Destroy(gameObject);
        }
    }
}
