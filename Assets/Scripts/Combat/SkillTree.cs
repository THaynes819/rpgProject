using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
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
        PlayerClass playerClass;

        [SerializeField] ActionSkill[] slots = null; // Current availablr Skills
        [SerializeField] int skillTreeSize = 16;

        //public SkillStruct slot;

        // [System.Serializable]
        // public struct SkillStruct
        // {
        //     public PlayerClass skillClass;
        //     public ActionSkill skill;
        //     public int levelRequired;
        //     public Sprite icon;
        //     public int number;

        // }

        // Makke it a true tree so a skill has prerquisites???
        // TODO Make Get all class skills Method

        public event Action skillTreeUpdated;

        public static SkillTree GetPlayerSkillTree ()
        {
            var player = GameObject.FindWithTag ("Player");
            return player.GetComponent<SkillTree> ();
        }

        public bool AddSkillToSlot (int slot, ActionSkill skill)
        {
            if (slots[slot] != null)
            {
                return AddToAssignedSlot (skill, 1);
            }

            slots[slot] = actionSkill;
            if (skillTreeUpdated != null)
            {
                skillTreeUpdated ();
            }
            return true;
        }

        public ActionSkill GetSkillInSlot (int slot)
        {
            return slots[slot];
        }

        public void RemoveFromSlot (int slot, int number)
        {
            if (slots[slot] != null)
            {

                slots[slot] = null;
            }
            if (skillTreeUpdated != null)
            {
                skillTreeUpdated ();
            }
        }

        public bool AddToAssignedSlot (ActionSkill skill, int number)
        {
            var assignedSlot = skill.GetSlot ();
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].GetSlot () != assignedSlot) return false;
            }

            if (skillTreeUpdated != null)
            {
                skillTreeUpdated ();
            }
            return true;
        }

        public int GetTreeSize ()
        {
            return skillTreeSize;
        }

        void Awake ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");
            slots = player.GetComponent<SkillTree>().slots;
            playerClass = player.GetComponent<BaseStats> ().GetPlayerClass ();
        }

        void Start ()
        {
            playerLevel = player.GetComponent<BaseStats> ().GetLevel ();
        }

        private bool CanLearn ()
        {
            var skillList = slots;
            foreach (var skill in skillList)
            {
                int skillLevel = skill.GetLevelRequired ();

                if (playerLevel < skillLevel) return false;

                Debug.Log (skill.name + "  The Player level is " + playerLevel + " and the Skill level is " + skill.GetLevelRequired ());
                Debug.Log ("made it past level rquired");
                if (playerClass != skill.GetSkillClass ()) return false;
                Debug.Log ("The player Class is " + playerClass + " and the Skill class is " + skill.GetSkillClass ());
                if (playerLevel >= skill.GetLevelRequired ()) return true;
                if (playerClass == skill.GetSkillClass ()) return true;

            }
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
                if (skillTreeUpdated != null)
                {
                    Debug.Log ("skillTreeUpdated was called");
                    skillTreeUpdated ();
                }
            }

        }

        public List<ActionSkill> GetAvailableSkills ()
        {
            return LearnableSkills ();
        }

        private List<ActionSkill> LearnableSkills ()
        {
            List<ActionSkill> learnableSkills = new List<ActionSkill> ();
            var skillList = Resources.LoadAll<ActionSkill> ("");

            foreach (ActionSkill skill in skillList)
            {
                if (CanLearn ())
                {
                    learnableSkills.Add (skill);
                }
                Debug.Log ("LearnableSkills is this long " + learnableSkills.Count);
                return learnableSkills;
            }
            if (skillTreeUpdated != null)
            {
                Debug.Log ("skillTreeUpdated was called");
                skillTreeUpdated ();
            }
            Debug.Log ("The learnable Skills list is this long after SkillTreeUpdated" + learnableSkills.Count);
            return learnableSkills;
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