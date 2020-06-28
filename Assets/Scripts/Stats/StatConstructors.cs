using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
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

    [System.Serializable]
    public class CastableSkill
    {
        [Header ("Castable Skill Info")]
        [SerializeField] float skillCost = 5f;
        [SerializeField] float skillRegeneration = 5f;
        [SerializeField] float skillCooldown = 5f;
        [SerializeField] float healingAmount = 20f;
        [SerializeField] float skillDamage = 15f;
        [SerializeField] float skillRange = 10f;
        [SerializeField] Health targetToDamage = null;

        public CastableSkill (float skillCost, float skillRegeneration, float skillCooldown, float healingAmount, float skillDamage, float skillRange, Health targetToDamage)
        {
            this.skillCost = skillCost;
            this.skillRegeneration = skillRegeneration;
            this.skillCooldown = skillCooldown;
            this.healingAmount = healingAmount;
            this.skillDamage = skillDamage;
            this.skillRange = skillRange;
            this.targetToDamage = targetToDamage;
        }
    }

}