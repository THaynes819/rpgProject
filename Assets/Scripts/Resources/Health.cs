using UnityEngine;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveableMe 
    {
        
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;
        float experienceReward = 0;        


        private void Start() 
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();            
        }    

        public bool IsDead()
        {
            return isDead;
        }       

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints <= 0)
            {
                experienceReward = GetComponent<BaseStats>().GetExperienceReward();                
                Die();
                AwardExperience(instigator);
            }            
        }

        public float GetPercentage()
        {
            return 100 * healthPoints / GetComponent<BaseStats>().GetHealth();
        }        

        private void Die()
        {
            if (isDead)  return;
            Collider collider = GetComponent<Collider>();
            Destroy(collider);
            isDead = true;
            
            
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentACtion(); 
        }

        private void AwardExperience(GameObject instigator)
        {            
            Experience experience = instigator.GetComponent<Experience>();
            if(experience == null)
            {
                return;
            }
            
                Debug.Log("the xp reward is " + experienceReward);
                Debug.Log("awarded to " + instigator);
                experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
            
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;            
            if (healthPoints <= 0)
            {
                Die();
            }
        }
    }
}