using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{

    [CreateAssetMenu (fileName = "PlayerClassSkillTree", menuName = "RPG/Class Skill Tree", order = 0)]
    public class PlayerClassSkillTree : ScriptableObject
    {
        public PlayerClass treeClass;

        public List<PlayerClassSkill> playerClassSkills = new List<PlayerClassSkill>();


        public PlayerClass GetTreeClass()
        {
            return treeClass;
        }

        public List<PlayerClassSkill> GetTreeList()
        {
            return playerClassSkills;
        }

    }
}