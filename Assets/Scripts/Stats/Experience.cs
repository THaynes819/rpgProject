using System;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;
        [SerializeField] float experienceDebugAmount = 100f;

        public event Action onExperienceGained;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GainExperience(experienceDebugAmount);
            }
        }

        public void GainExperience (float experience) 
        {
            var player = GameObject.FindWithTag("Player");
            BaseStats baseStats = player.GetComponent<BaseStats>();
            if (baseStats.GetLevel() < baseStats.GetMaxLevel())
            {
                experiencePoints += experience;
            onExperienceGained();
            }
            
            else
            {
                Debug.Log("Already at Max Level");
            }
        }

        public float GetExperiencePoints () {
            return experiencePoints;
        }

        public object CaptureState () {
            return experiencePoints;
        }

        public void RestoreState (object state)
        {
            experiencePoints = (float) state;
        }
    }
}