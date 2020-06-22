using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Caster : MonoBehaviour, IAction
    {

        [SerializeField] GameObject spellCaster;
        [SerializeField] Spell spell = null;
        [SerializeField] Transform spellOrigin = null;
        [SerializeField] GameObject preCastAnimation = null;
        public bool hasProjectile = true;

        public float timeSinceLastCast = Mathf.Infinity;

        ResourcePool resourcePool;
        Animator animator;
        Health target;
        Stat playerPool;
        bool isCorrectClass = true;

        void Awake ()
        {
            animator = GetComponent<Animator> ();

        }

        void Start ()
        {

            SpellAnimator ();
        }

        void Update ()
        {
            playerPool = spellCaster.GetComponent<ResourcePool> ().GetCurrentPool (); // Remove after UI Creation is setup or use Observer Method
            UpdateTimers ();

            if (target == null) return;
            Debug.Log ("The Player Pool is " + playerPool + " and the spell Resource Type is " + spell.resourceType);
            if (target.IsDead ()) return;
            if (spell.GetResourceType () != playerPool) { Debug.Log ("You cannot cast that"); isCorrectClass = false; }
            if (spell.GetResourceType () == playerPool) { Debug.Log ("You Have the correct resource"); isCorrectClass = true; }

            if (!GetIsInRange (target.transform))
            {
                GetComponent<Mover> ().MoveTo (target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover> ().Cancel ();
                CastingBehaviour ();
            }

        }

        private void UpdateTimers ()
        {
            timeSinceLastCast += Time.deltaTime;
        }

        public bool CanCast ()
        {

            float resourcePoints = spellCaster.GetComponent<ResourcePool> ().GetCurrentResourcePoints ();
            if (timeSinceLastCast < spell.GetSpellCoolDown ()) return false;
            if (spell.GetSpellCost () > resourcePoints) return false;
            Debug.Log ("Spel Cost is " + spell.GetSpellCost () + " Resource Points are " + resourcePoints);
            //Debug.Log ("Got past Spell Cost");
            if (!isCorrectClass) return false;
            //Debug.Log ("Got Past Correct Class!");
            if (target == null) return false;
            if (target.IsDead ()) return false;

            return true;
        }

        private void CastingBehaviour ()
        {
            transform.LookAt (target.transform);
            if (CanCast ())
            {
                SpellAnimator ();
                TriggerSpell ();
                timeSinceLastCast = 0;
            }

        }

        private void TriggerSpell ()
        {
            GetComponent<Animator> ().ResetTrigger ("stopAttack");
            GetComponent<Animator> ().SetTrigger ("attack");
        }

        //Animation Event
        void CastSpell ()
        {

            var resourcePool = spellCaster.GetComponent<ResourcePool> ();

            if (target == null) return;
            if (spell.isPoolGenerating)
            {
                //resourcePoints += spell.GetSpellResourceGeneration ();
                resourcePool.SetCurrentResourcePoints (spell.GetSpellResourceGeneration ());
            }
            if (!spell.isPoolGenerating)
            {
                resourcePool.SetCurrentResourcePoints (-spell.GetSpellCost ());
            }
            if (hasProjectile)
            {
                Projectile spellInstance = Instantiate (spell.GetProjectile (), spellOrigin.position, Quaternion.identity);
                spellInstance.SetTarget (target, gameObject, spell.GetSpellDamage ());
            }
            else
            {
                target.TakeDamage (gameObject, spell.GetSpellDamage ());
            }

        }

        public AnimatorOverrideController SpellAnimator ()
        {
            var animatorOverride = spell.GetOverrideController ();

            if (preCastAnimation != null)
            {
                preCastAnimation.GetComponent<Animator> ().enabled = true;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            return animatorOverride;
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