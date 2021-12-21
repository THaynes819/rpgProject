using System;
using System.Collections;
using GameDevTV.Saving;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Pools
{
    public class Health : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        [Range (1, 100)]
        [SerializeField] float levelUpHealthPercent = 90f;
        [SerializeField] TakeDamageEvent takeDamage;        
        [SerializeField] bool isMobToKill = false;
        [SerializeField] string questName;
        [SerializeField] string questObjective;
        public UnityEvent onDeath;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

        [SerializeField] Condition condition;

        LazyValue<float> healthPoints;

        bool wasDeadlastFrame = false;
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
            return healthPoints.value <= 0;
        }

        public void SetIsDead(bool value)
        {
            wasDeadlastFrame = value;
        }

        public void TakeDamage (GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max (healthPoints.value - damage, 0);

            if (IsDead())
            {
                Debug.Log("onDeath should invoke because this died: " + this.name);
                onDeath.Invoke (); //TODO fix this so that the SFX doesn't instantiate on dead mobs
                AwardExperience (instigator);
            }
            else
            {
                takeDamage.Invoke (damage);
            }
            UpdateState ();
        }

        public void Heal (float healthToRestore)
        {
            healthPoints.value = Mathf.Min (healthPoints.value + healthToRestore, GetMaxHealthPoints ());
            UpdateState ();
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

        public GameObject GetInstigator (GameObject instigator)
        {
            return instigator;
        }

        public bool? Evaluate (Predicates predicate, string[] parameters)
        {
            if (predicate == Predicates.KillForQuest && parameters[0] == questName)
            {

                return isMobToKill && IsDead ();
            }

            return null;
        }

        

        private void UpdateState ()
        {
            Animator animator = GetComponent<Animator> ();
            if (isMobToKill) // if it's the quest mob to kill. This needs to be changed. It's not good. Check the code, This makes no sense. Death Aounouncer anounces the death to quest system?
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<IDeathAnnouncer> ().DeathAnnounce (questName, questObjective);
            }
            // Collider collider = GetComponent<Collider> ();
            // Rigidbody rigidbody = GetComponent<Rigidbody> ();
            // Destroy (collider);
            // Destroy (rigidbody);

            if (!wasDeadlastFrame && IsDead())
            {
                animator.SetTrigger ("die");
                GetComponent<ActionScheduler> ().CancelCurrentACtion ();
            }

            if (wasDeadlastFrame && !IsDead())
            {
                animator.Rebind();
            }

            

            wasDeadlastFrame = IsDead();
        }

        private void AwardExperience (GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience> ();
            if (experience == null)
            {
                return;
            }
            BaseStats baseStats = instigator.GetComponent<BaseStats>();
            if (baseStats.GetLevel() >= baseStats.GetMaxLevel())
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
            
            UpdateState();
        }

    }
}