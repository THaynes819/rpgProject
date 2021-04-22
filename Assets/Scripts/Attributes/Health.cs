using System;
using GameDevTV.Utils;
using RPG.Core;
using GameDevTV.Saving;
using RPG.Stats;
using RPG.UI.DamageText;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [Range (1, 100)]
        [SerializeField] float levelUpHealthPercent = 90f;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent deathEvent;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

        LazyValue<float> healthPoints;

        bool isDead = false;
        float experienceReward = 0;

        void Awake ()
        {
            healthPoints = new LazyValue<float> (GetInitialhealth);
        }

        private float GetInitialhealth ()
        {
            return GetComponent<BaseStats> ().GetStat (Stat.Health);
        }

        private void Start ()
        {
            healthPoints.ForceInit ();

        }

        private void OnEnable ()
        {
            GetComponent<BaseStats> ().onLevelUp += RegenerateHealth;
        }

        private void OnDisable ()
        {
            GetComponent<BaseStats> ().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead ()
        {
            return isDead;
        }

        public void TakeDamage (GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max (healthPoints.value - damage, 0);

            if (Mathf.Approximately (healthPoints.value, 0))
            {
                deathEvent.Invoke (); //TODO fix this so that the SFX doesn't instantiate on dead mobs
                Die ();
                AwardExperience (instigator);
            }
            else
            {
                takeDamage.Invoke (damage);
            }
        }

        public void Heal (float healthToRestore)
        {

            healthPoints.value = Mathf.Min (healthPoints.value + healthToRestore, GetMaxHealthPoints ());
        }

        public float GetPercentage ()
        {
            return 100 * GetFraction ();
        }

        public float GetFraction ()
        {
            return healthPoints.value / GetComponent<BaseStats> ().GetStat (Stat.Health);
        }

        public float GetHealthPoints ()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints ()
        {
            return GetComponent<BaseStats> ().GetStat (Stat.Health);
        }

        // public GameObject GetInstigator(GameObject instigator)
        // {
        //     return instigator;
        // }

        private void Die ()
        {
            if (isDead) return;
            Collider collider = GetComponent<Collider> ();
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            Destroy (collider);
            Destroy (rigidbody);  //TODO make this destroyer after the death animation completes
            isDead = true;
            GetComponent<Animator> ().SetTrigger ("die");
            GetComponent<ActionScheduler> ().CancelCurrentACtion ();
        }

        private void AwardExperience (GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience> ();
            if (experience == null)
            {
                return;
            }

            experience.GainExperience (GetComponent<BaseStats> ().GetStat (Stat.ExperienceReward));
        }

        private void RegenerateHealth ()
        {
            float newHealthPoints = GetComponent<BaseStats> ().GetStat (Stat.Health) * (levelUpHealthPercent / 100);
            healthPoints.value = Mathf.Max (healthPoints.value, newHealthPoints);
        }

        public object CaptureState ()
        {
            return healthPoints.value;
        }

        public void RestoreState (object state)
        {
            healthPoints.value = (float) state;
            if (Mathf.Approximately (healthPoints.value, 0))
            {
                Die ();
            }
        }
    }
}