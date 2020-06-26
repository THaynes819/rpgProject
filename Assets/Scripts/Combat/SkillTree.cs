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

        GameObject player;
        int playerLevel;
        int index;
        BaseStats playerStats;
        ActionSkill actionSkill;
        PlayerClass playerClass;
        Dictionary<int, DockedItemSlot> allSkills = null;
        //List<ActionSkill> skillBook;

        [SerializeField] ActionSkill[] slots = null; // Current available Skills
        [SerializeField] int skillTreeSize = 16;

        Dictionary<int, DockedItemSlot> skillBook = new Dictionary<int, DockedItemSlot> ();
        public class DockedItemSlot
        {
            public ActionSkill item;
            public int number;
        }

        // Make it a true tree so a skill has prerquisites???

        public event Action skillTreeUpdated;

        public static SkillTree GetPlayerSkillTree ()
        {
            var player = GameObject.FindWithTag ("Player");
            return player.GetComponent<SkillTree> ();
        }

        void Awake ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");
            slots = player.GetComponent<SkillTree> ().slots;
            playerClass = player.GetComponent<BaseStats> ().GetPlayerClass ();
            BuildAllSkills ();
        }

        void Start ()
        {
            playerLevel = player.GetComponent<BaseStats> ().GetLevel ();
            PopulateSkillBook ();
        }

        public void StoreInSlot (InventoryItem item, int index)
        {

            if (!skillBook.ContainsKey (index))
            {
                var slot = new DockedItemSlot ();
                slot.item = item as ActionSkill;
                skillBook[index] = slot;
            }

            if (skillTreeUpdated != null)
            {
                skillTreeUpdated ();
            }
        }

        public void RemoveFromSlot (int index)
        {
            Debug.Log("Skill Tree Removed an item");
            if (skillBook.ContainsKey (index))
            {
                skillBook.Remove (index);
            }
            if (skillTreeUpdated != null)
            {
                skillTreeUpdated ();
            }
        }

        public int MaxAcceptable (InventoryItem item, int index)
        {
            if (!actionSkill) return 0;
            if (skillBook.ContainsKey (index) && !object.ReferenceEquals (item, skillBook[index].item))
            {
                return 0;
            }
            if (skillBook.ContainsKey (index))
            {
                return 0;
            }

            return 1;
        }

        public Dictionary<int, DockedItemSlot> GetAllslots ()
        {
            return allSkills;
        }

        public int GetTreeSize ()
        {
            return skillTreeSize;
        }

        public InventoryItem GetSkillinSlot (int index)
        {
            if (skillBook.ContainsKey (index))
            {
                return skillBook[index].item;
            }
            return null;
        }

        public int GetNumberInSlot(int index)
        {
            if (skillBook.ContainsKey (index))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private void BuildAllSkills ()
        {

            if (allSkills != null)
            {
                return;
            }

            allSkills = new Dictionary<int, DockedItemSlot> ();
            foreach (var slot in slots)
            {
                DockedItemSlot docked = new DockedItemSlot();
                docked.item = slot;
                docked.number = 1;
                allSkills.Add(slot.GetSlot(), docked);

                // Debug.Log("The skill is " + docked.item);
                // Debug.Log ("The slot is " + slot.GetSlot());

                // Debug.Log ("the Get level in Skills are " + slot.GetLevelRequired());
            }

            if (skillTreeUpdated != null)
            {
                skillTreeUpdated ();
            }
        }

        private void LearnSkill ()
        {
            PopulateSkillBook ();
        }

        private void PopulateSkillBook ()
        {
            foreach (var skill in allSkills)
            {
                var skillLevel = skill.Value.item.GetLevelRequired();
                var skillClass = skill.Value.item.GetSkillClass();
                if (playerLevel >= skillLevel && playerClass == skillClass)
                {
                    skillBook.Add(skill.Key, skill.Value);
                }
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