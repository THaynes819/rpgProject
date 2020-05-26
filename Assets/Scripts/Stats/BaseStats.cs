using UnityEngine;


namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] Stat stat;
        [Range(1, 99)]
        [SerializeField] int currentLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        
        void Update()
        {
            if(gameObject.tag == "Player")
            {
            Debug.Log(GetLevel());            
            }
        }

        public float GetStat(Stat stat)
        {
            if (gameObject.tag == "Player")
            {
                currentLevel = GetLevel();
                //return progression.GetStat(stat, characterClass, GetLevel());            
            }
            return progression.GetStat(stat, characterClass, currentLevel);            
        }

        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();

            if(experience == null)
            {
                return currentLevel;
            }
            
            float currentXP = experience.GetExperiencePoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float xPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if(xPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;            
        }
    }
}
