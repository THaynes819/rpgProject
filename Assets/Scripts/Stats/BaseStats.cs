using System;
using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        [SerializeField] Stat stat;
        [Range (1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelupEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onLevelUp;

        int currentLevel = 0;

        private void Start () {
            currentLevel = CalculateLevel ();
            Experience experience = GetComponent<Experience> ();
            if (experience != null) {
                experience.onExperienceGained += UpdateLevel;
                //To Do Remember that Events dont require Methods to 
                //have () to be called! Because is adding the code not calling the method
            }
        }

        private void UpdateLevel () {
            int newLevel = CalculateLevel ();
            if (newLevel > currentLevel) {
                currentLevel = newLevel;
                LevelUpEffect ();
                onLevelUp ();
            }
        }

        private void LevelUpEffect () {
            GameObject newlevelUpEffect = Instantiate (levelupEffect, transform);
        }

        public float GetStat (Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }

        

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private int GetLevel () 
        {
            if (currentLevel < 1) 
            {
                currentLevel = CalculateLevel ();
            }
            return currentLevel;
        }

        private float GetAdditiveModifier (Stat stat)
        {
            if (!shouldUseModifiers)
            {
                return 0;
            }
            
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers)
            {
                return 0;
            }

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private int CalculateLevel () {
            Experience experience = GetComponent<Experience> ();
            if (experience != null) {
                float currentXP = experience.GetExperiencePoints ();
                int penultimateLevel = progression.GetLevels (Stat.ExperienceToLevelUp, characterClass);
                for (int level = 1; level <= penultimateLevel; level++) {
                    float XPToLevelUp = progression.GetStat (Stat.ExperienceToLevelUp, characterClass, level);
                    if (XPToLevelUp > currentXP) {
                        return level;
                    }
                }
                return penultimateLevel + 1;
            } else {
                return startingLevel;
                //TO DO Set a bool to set enemys to either have level calculated or set in inspector
            }
        }
    }
}