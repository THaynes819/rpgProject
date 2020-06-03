using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.UI.DamageText;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveableMe
    {
        [Range (1, 100)]
        [SerializeField] float levelUpHealthPercent = 90f;
        [SerializeField] TakeDamageEvent takeDamage;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        { }

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

            if (healthPoints.value <= 0)
            {
                Die (instigator);
            }
            else
            {
                takeDamage.Invoke (damage);
            }
        }

        public float GetPercentage ()
        {
            return 100 * GetFraction();
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

        private void Die (GameObject instigator)
        {
            if (isDead) return;
            Collider collider = GetComponent<Collider> ();
            Destroy (collider);
            if (instigator != null)
            {
                AwardExperience (instigator);
            }
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
            return healthPoints;
        }

        public void RestoreState (object state)
        {
            GameObject instigator = null;
            healthPoints.value = (float) state;
            if (healthPoints.value <= 0)
            {
                Die (instigator);
            }
        }
    }
}