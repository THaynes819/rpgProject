using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Pools;
using UnityEngine;

namespace RPG.Stats
{

    public class SkillTree : MonoBehaviour, IModifierProvider

    {
        GameObject player;
        ActionSkill actionSkill;
        BaseStats baseStats;
        List<PlayerClassSkill> skillBook = new List<PlayerClassSkill> ();
        List<ActionSkill> AvailableSkills = new List<ActionSkill> ();
        ActionSkill.Modifier[] additiveModifiers;
        ActionSkill.Modifier[] percentModifiers;

        public List<SkillTrees> skillTrees = new List<SkillTrees> ();
        [SerializeField] int skillTreeSize = 16;

        //public event Action skillTreeUpdated; //TODO Remove this as it appears to do nothing... Maybe revamp this for leveling up???

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
            // if (baseStats != null)
            // {
            //     baseStats.onLevelUp += skillTreeUpdated;
            // }
        }
        private void OnDisable ()
        {
            // if (baseStats != null)
            // {
            //     baseStats.onLevelUp -= skillTreeUpdated;
            // }
        }

        public int GetTreeSize ()
        {
            return skillTreeSize;
        }

        public SkillTree GetSkillTree ()
        {
            if (player.GetComponent<SkillTree> () != null)
            {
                return GetComponent<SkillTree> ();
            }
            else
            {
                Debug.Log("Player SkillTree is null within itself...you know somehow");
                return null;
            }

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
            // if (skillTreeUpdated != null)
            // {
            //     skillTreeUpdated ();
            // }

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

        public void ToggleSkill (int index, bool toggle)
        {

            if (toggle)
            {
                foreach (var skill in AvailableSkills)
                {
                    if (index == skill.GetSlot ())
                    {
                        additiveModifiers = new ActionSkill.Modifier[skill.GetSkillAddModifiers ().Length];
                        additiveModifiers = skill.GetSkillAddModifiers ();
                        percentModifiers = new ActionSkill.Modifier[skill.GetSkillAddModifiers ().Length];
                        percentModifiers = skill.GetSkillPercentModifiers ();
                    }

                }
            }
            if (!toggle)
            {
                Debug.Log ("Toggled off");
            }
        }

        public IEnumerable<float> GetAdditiveModifiers (Stat stat)
        {
            if (additiveModifiers != null)
            {
                foreach (var modifier in additiveModifiers)
                {
                    if (modifier.stat == stat)
                    {
                        yield return modifier.value;
                    }
                }
            }
        }

        public IEnumerable<float> GetPercentageModifiers (Stat stat)
        {
            if (percentModifiers != null)
            {
                foreach (var modifier in percentModifiers)
                {
                    if (modifier.stat == stat)
                    {
                        yield return modifier.value;
                    }
                }
            }
        }
    }
}