using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Caster : MonoBehaviour, IAction
    {

        [SerializeField] GameObject spellCaster;
        [SerializeField] Spell spell = null;
        [SerializeField] Transform spellOrigin = null;

        public float timeSinceLastCast = Mathf.Infinity;

        Health target;

        void Update ()
        {
            UpdateTimers ();

            if (target == null) return;

            if (target.IsDead ()) return;
            if (!GetIsInRange (target.transform))
            {
                GetComponent<Mover> ().MoveTo (target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover> ().Cancel ();

                var instigator = gameObject;
                CastAttack (spellOrigin, target, instigator);
            }

        }

        private void UpdateTimers ()
        {
            timeSinceLastCast += Time.deltaTime;
        }

        public bool CanCast ()
        {
            float resourcePoints = spellCaster.GetComponent<ResourcePool> ().GetCurrentResourcePoints ();

            if (spell.GetSpellCost () > resourcePoints) return false;
            if (target == null) return false;
            if (target.IsDead ()) return false;

            return true;
        }

        private void CastingBehaviour ()
        {
            transform.LookAt (target.transform);
            if (timeSinceLastCast > spell.GetSpellCoolDown ())
            {
                TriggerSpell ();
                timeSinceLastCast = 0;
            }

        }

        private void TriggerSpell ()
        {
            GetComponent<Animator> ().ResetTrigger ("stopAttack");
            GetComponent<Animator> ().SetTrigger ("attack");
        }

        //Animation Event???
        void CastAttack (Transform spellOrigin, Health target, GameObject instigator)
        {
            if (CanCast ())
            {
                Projectile spellInstance = Instantiate (spell.GetProjectile (), spellOrigin.position, Quaternion.identity);
                spellInstance.SetTarget (target, instigator, spell.GetSpellDamage ());
            }
        }

        public void Cast (GameObject combatTarget)
        {
            GetComponent<ActionScheduler> ().StartAction (this);
            target = combatTarget.GetComponent<Health> ();
        }

        private bool GetIsInRange (Transform targetTransform)
        {
            return Vector3.Distance (transform.position, targetTransform.position) < spell.GetSpellRange ();
        }

        public void Cancel ()
        {
            target = null;
            GetComponent<Mover> ().Cancel ();
        }

        private void StopSpell ()
        {
            GetComponent<Animator> ().ResetTrigger ("attack");
            GetComponent<Animator> ().SetTrigger ("stopAttack");
        }
    }

}