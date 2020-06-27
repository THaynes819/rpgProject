using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Stats
{

    public class SkillTree : MonoBehaviour

    {
        GameObject player;
        BaseStats playerStats;
        ActionSkill actionSkill;
        PlayerClass playerClass;
        List<PlayerClassSkill> skillBook = new List<PlayerClassSkill> ();
        List<ActionSkill> AvailableSkills = new List<ActionSkill>();

        public List<SkillTrees> skillTrees = new List<SkillTrees> ();

        [SerializeField] int skillTreeSize = 16;

        // Make it a true tree so a skill has prerquisites???

        public event Action skillTreeUpdated;

        void Start ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");
            FindTree ();
        }

        public int GetTreeSize ()
        {
            return skillTreeSize;
        }

        public SkillTree GetSkillTree ()
        {
            return this;
        }

        public List<ActionSkill> GetSkillBook()
        {
            return AvailableSkills;
        }

        public void FindTree ()
        {
            var treeList = Resources.LoadAll<PlayerClassSkillTree> ("");
            playerClass = player.GetComponent<BaseStats> ().GetPlayerClass ();
            foreach (var tree in treeList)
            {
                if (playerClass == tree.GetTreeClass ())
                {
                    tree.GetTreeList ();
                    skillBook = tree.GetTreeList ();
                }
            }
            Debug.Log ("The Skill Book has " + skillBook.Count);
            HandleSkillBook ();
        }

        private void HandleSkillBook ()
        {

            foreach (var skill in skillBook)
            {
                if (CanLearn (skill.skill))
                {
                    AvailableSkills.Add(skill.skill);
                }
            }
        }

        private bool CanLearn (ActionSkill skill)
        {
            int playerLevel = player.GetComponent<BaseStats> ().GetLevel ();
            if (playerLevel < skill.GetLevelRequired ())
            {
                return false;
            }
            return true;
        }

        // private void OnEnable ()   //TODO Fix Race Condition
        // {
        //     player.GetComponent<BaseStats> ().onLevelUp += LearnSkill;
        // }
        // private void OnDisable ()
        // {
        //     player.GetComponent<BaseStats> ().onLevelUp -= LearnSkill;
        // }
    }
}