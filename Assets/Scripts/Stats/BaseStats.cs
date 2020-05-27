using UnityEngine;


namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] Stat stat;
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        
        int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();            
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {                
                experience.onExperienceGained  += UpdateLevel;
                Debug.Log("I am not an NPC" + gameObject.name);
                Debug.Log("The current Level should be " + currentLevel);
                //To Do Remember that Events dont require Methods to 
                //have () to be called! Because is adding the code not calling the method
            }
        }
        
        void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
                print("Leveled Up");
            }
        }

        public float GetStat(Stat stat)
        {            
            return progression.GetStat(stat, characterClass, GetLevel());            
        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        public int CalculateLevel()
        {            
            Experience experience = GetComponent<Experience>();
            if(experience != null)
            {
                Debug.Log("Calculate Level Was called");            
            
                float currentXP = experience.GetExperiencePoints();
                Debug.Log("The calculate XP is " + currentXP);
                int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
                for (int level = 1; level <= penultimateLevel; level++)
                {
                    float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                    if(XPToLevelUp > currentXP)
                    {
                        return level;
                    }
                }
                return penultimateLevel + 1;
            }
            else
            {
                return startingLevel;
                //TO DO Set a bool to set enemys to either have level calculated or set in inspector
            }          
        }
    }
}
