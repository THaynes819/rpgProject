using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{

    public class SkillTree : MonoBehaviour
    {

        bool canLearn = false;
        GameObject player;
        int playerLevel;
        BaseStats playerStats;
        ActionSkill actionSkill;

        [SerializeField] SkillToLearn[] skillTable = null; // Data for all Skills in the game

        [System.Serializable]
        struct SkillToLearn
        {
            public PlayerClass skillClass;
            public ActionSkill actionSkill;
            public int levelRequired;
        }

        // Makke it a true tree so a skill has prerquisites???
        // TODO Make Get all class skills Method

        public static SkillTree GetPlayerSkillTree ()
        {
            var player = GameObject.FindWithTag ("Player");
            return player.GetComponent<SkillTree> ();
        }

        void Awake ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");
        }

        void Start ()
        {
            playerLevel = player.GetComponent<BaseStats> ().GetLevel ();
        }

        public List<ActionSkill> GetAvailableSkills ()
        {
            List<ActionSkill> AvailableSkills = new List<ActionSkill> ();
            return AvailableSkills;
        }

        private List<ActionSkill> LearnableSkills ()
        {
            List<ActionSkill> learnableSkills = new List<ActionSkill> ();
            var playerClass = player.GetComponent<BaseStats> ().GetPlayerClass ();
            bool islevelAndClass = true;
            bool hasLearned = false;
            foreach (SkillToLearn skill in skillTable)
            {
                if (playerLevel < skill.levelRequired || playerClass != skill.skillClass)
                {
                    Debug.Log ("The skills class is " + skill.skillClass + " The player class is " + playerClass);
                    Debug.Log ("The player's level is " + playerLevel + " The level required is " + skill.levelRequired);
                    islevelAndClass = false;
                }

                if (GetAvailableSkills ().Contains (skill.actionSkill))
                {
                    hasLearned = true;
                }
                if (islevelAndClass && hasLearned == false)
                {
                    Debug.Log ("canLearnSkill is " + skill.actionSkill + islevelAndClass + hasLearned + " (TrueFalse is good)");
                    learnableSkills.Add (skill.actionSkill);
                    GetAvailableSkills ().Add (skill.actionSkill);
                }
            }
            return learnableSkills;
        }

        private bool CanLearn ()
        {
            if (LearnableSkills () == null) return false;
            return true;
        }

        private void LearnSkill ()
        {
            if (!CanLearn ())
            {
                Debug.Log ("Did not learn a Skill");
            }
            foreach (var i in LearnableSkills ())
            {
                Debug.Log ("Learned a skill " + i.GetDisplayName ());
            }

        }

        private void OnEnable ()
        {
            GetComponent<BaseStats> ().onLevelUp += LearnSkill;
        }

        private void OnDisable ()
        {
            GetComponent<BaseStats> ().onLevelUp -= LearnSkill;
        }
    }
}