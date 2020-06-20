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
        [SerializeField] GameObject preCastAnimation = null;

        public float timeSinceLastCast = Mathf.Infinity;

        Animator animator;
        Health target;

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
            if (target == null) return false;
            if (target.IsDead ()) return false;

            return true;
        }

        private void CastingBehaviour ()
        {
            transform.LookAt (target.transform);
            if (CanCast ())
            {
                Debug.Log ("Casting Behaviour is past CanCast");
                TriggerSpell ();
                timeSinceLastCast = 0;
            }

        }

        private void TriggerSpell ()
        {
            Debug.Log ("Trigger Spell is Being Called");
            GetComponent<Animator> ().ResetTrigger ("stopAttack");
            GetComponent<Animator> ().SetTrigger ("attack");
        }

        //Animation Event???
        void Shoot ()
        {
            Debug.Log ("Shoot is being Called");
            Projectile spellInstance = Instantiate (spell.GetProjectile (), spellOrigin.position, Quaternion.identity);
            spellInstance.SetTarget (target, gameObject, spell.GetSpellDamage ());

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