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
                float experieceGain = Time.deltaTime * experienceDebugAmount;
                GainExperience(experieceGain);
            }
        }

        public void GainExperience (float experience) {
            experiencePoints += experience;
            onExperienceGained();
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