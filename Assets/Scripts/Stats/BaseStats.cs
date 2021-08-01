using System;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] Stat stat;
        [SerializeField] Pool pool;
        [Range (1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelupEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        float currentXP;

        public event Action onLevelUp;

        LazyValue<int> currentLevel;

        Experience experience;

        public int GetLevel ()
        {
            return currentLevel.value;
        }

        public PlayerClass GetPlayerClass ()
        {
            PlayerClass playerClass = GetComponent<CharacterCreator> ().GetPlayerClass ();
            return playerClass;
        }

        public CharacterClass GetCharacterClass ()
        {
            return characterClass;
        }

        public float GetStat (Stat stat)
        {
            return (GetBaseStat (stat) + GetAdditiveModifier (stat)) * (1 + GetPercentageModifier (stat) / 100);
        }

        public Progression GetProgression ()
        {
            return progression;
        }

        public float GetExperienceFraction ()
        {
            int currentLevel = GetLevel ();
            float currenTotaltXP = experience.GetExperiencePoints ();
            float pastLevelXP = 0;
            float currentLevelXP = 0;
            float xPToLevelUp = progression.GetStat (Stat.ExperienceToLevelUp, characterClass, GetLevel ());
            float currentXPToLevelUp = 0;

            if (currentLevel > 1)
            {

                for (var i = 1; i < currentLevel - 1; i++)
                {
                    pastLevelXP += progression.GetStat (Stat.ExperienceToLevelUp, characterClass, i);
                }

                currentLevelXP = currenTotaltXP - pastLevelXP;
                currentXPToLevelUp = xPToLevelUp - pastLevelXP;

                if (currentLevelXP < 0)
                {
                    currentLevelXP = 0;
                }
            }
            else
            {
                currentLevelXP = experience.GetExperiencePoints ();
                currentXPToLevelUp = xPToLevelUp;
            }

            float experiencFraction = currentLevelXP / currentXPToLevelUp;

            return experiencFraction;
        }

        private void Awake ()
        {
            experience = GetComponent<Experience> ();
            currentLevel = new LazyValue<int> (CalculateLevel);
        }

        private void Start ()
        {
            currentLevel.ForceInit ();

        }

        private void OnEnable ()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable ()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel ()
        {
            int newLevel = CalculateLevel ();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect ();
                onLevelUp ();
            }
        }

        private void LevelUpEffect ()
        {
            GameObject newlevelUpEffect = Instantiate (levelupEffect, transform);
        }

        private float GetBaseStat (Stat stat)
        {
            return progression.GetStat (stat, characterClass, GetLevel ());
        }

        private float GetAdditiveModifier (Stat stat)
        {
            if (!shouldUseModifiers)
            {
                return 0;
            }

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider> ())
            {
                foreach (float modifier in provider.GetAdditiveModifiers (stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier (Stat stat)
        {
            if (!shouldUseModifiers)
            {
                return 0;
            }

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider> ())
            {
                foreach (float modifier in provider.GetPercentageModifiers (stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private int CalculateLevel ()
        {
            Experience experience = GetComponent<Experience> ();
            if (experience != null)
            {
                float currentXP = experience.GetExperiencePoints ();
                int penultimateLevel = progression.GetLevels (Stat.ExperienceToLevelUp, characterClass);
                for (int level = 1; level <= penultimateLevel; level++)
                {
                    float XPToLevelUp = progression.GetStat (Stat.ExperienceToLevelUp, characterClass, level);
                    if (XPToLevelUp > currentXP)
                    {
                        return level;
                    }
                }
                return penultimateLevel + 1;
            }
            else
            {
                return startingLevel;
                //TODO Set a bool to set enemys to either have level calculated or set in inspector
            }
        }
    }
}