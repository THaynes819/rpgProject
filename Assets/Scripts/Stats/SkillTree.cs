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
        ActionSkill actionSkill;
        BaseStats baseStats;
        List<PlayerClassSkill> skillBook = new List<PlayerClassSkill> ();
        List<ActionSkill> AvailableSkills = new List<ActionSkill> ();

        public List<SkillTrees> skillTrees = new List<SkillTrees> ();
        [SerializeField] int skillTreeSize = 16;

        public event Action skillTreeUpdated;

        void Awake ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");
            baseStats = player.GetComponent<BaseStats> ();
        }

        void Start ()
        {
            FindTree ();
        }

        private void OnEnable ()
        {
            if (baseStats != null)
            {
                baseStats.onLevelUp += skillTreeUpdated;
            }
        }
        private void OnDisable ()
        {
            if (baseStats != null)
            {
                baseStats.onLevelUp -= skillTreeUpdated;
            }
        }

        public int GetTreeSize ()
        {
            return skillTreeSize;
        }

        public SkillTree GetSkillTree ()
        {
            return player.GetComponent<SkillTree> ();
        }

        public List<ActionSkill> GetSkillBook ()
        {
            return AvailableSkills;
        }

        public void FindTree ()
        {
            var treeList = Resources.LoadAll<PlayerClassSkillTree> ("");
            var playerClass = baseStats.GetPlayerClass ();
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
                    AvailableSkills.Add (skill.skill);
                }
            }
            if (skillTreeUpdated != null)
            {
                skillTreeUpdated ();
            }

        }

        private bool CanLearn (ActionSkill skill)
        {
            int playerLevel = baseStats.GetLevel ();
            if (playerLevel < skill.GetLevelRequired ())
            {
                return false;
            }
            return true;
        }

        public ActionSkill GetSkillInSlot (int slot)
        {
            foreach (var skill in AvailableSkills)
            {
                if (skill.GetSlot () == slot)
                {
                    return skill;
                }

            }
            return null;
        }

        public void OnSkillSelect (int index, bool toggle)
        {
            Debug.Log ("Skill Slot says the index is " + index + " and toggle is " + toggle);

            foreach (var skill in AvailableSkills)
            {
                if (index == skill.GetSlot ())
                {
                    Debug.Log ("initial Toggle");
                    toggle = !toggle;
                }
            }
        }

        public void HandleButtonPress (int index, bool toggle)
        {
            IToggleable button = GetComponent<IToggleable> ();
            Debug.Log ("Skill Tree says the index is " + index + " and toggle is " + toggle);
        }

    }
}