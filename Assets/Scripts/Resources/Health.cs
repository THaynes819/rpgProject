using UnityEngine;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveableMe 
    {
        
        float healthPoints = -1f;

        bool isDead = false;
        float experienceReward = 0;
        
        

        private void Start() 
        {
            if(healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }            
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
                Die(instigator);
                
            }            
        }

        public float GetPercentage()
        {
            return 100 * healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health);
        }        

        private void Die(GameObject instigator)
        {
            if (isDead)  return;
            Collider collider = GetComponent<Collider>();
            Destroy(collider);
            if(instigator != null)
            {
                AwardExperience(instigator);
            }
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

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
            
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            GameObject instigator = null;
            healthPoints = (float)state;            
            if (healthPoints <= 0)
            {
                Die(instigator);
            }
        }
    }
}