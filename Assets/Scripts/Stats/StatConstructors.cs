using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [System.Serializable]
    public class PlayerClassSkill
    {
        public PlayerClass playerClass;
        public ActionSkill skill;
        int level;
        public int charges;

        public PlayerClassSkill (ActionSkill skill, int charges, PlayerClass playerClass, int level)
        {
            this.skill = skill;
            this.charges = charges;
            this.playerClass = playerClass;
            this.level = level;
        }
    }

    [System.Serializable]
    public class SkillTrees
    {
        public PlayerClassSkillTree playerClassSkillTree;
        public PlayerClass skillTreeClass;

        public SkillTrees (PlayerClassSkillTree playerClassSkillTree, PlayerClass skillTreeClass)
        {
            this.playerClassSkillTree = playerClassSkillTree;
            this.skillTreeClass = skillTreeClass;
        }
    }

}