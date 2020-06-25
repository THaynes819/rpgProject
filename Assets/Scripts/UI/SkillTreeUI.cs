using System.Collections;
using System.Collections.Generic;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.UI.Inventories;
using RPG.Combat;
using UnityEngine;

namespace RPG.UI
{
    public class SkillTreeUI : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] ActionSkillSlotUI actionSkillPrefab = null;

        // CACHE
        ActionSkill actionSkill;
        SkillTree skillTree;
        ActionSkillIcon skillIcon;
        List<ActionSkill> availableskills;

        void Awake ()
        {
            skillTree = SkillTree.GetPlayerSkillTree ();
            availableskills = skillTree.GetAvailableSkills ();
            skillTree.skillTreeUpdated += Redraw;
        }
        private void Start ()
        {
            Redraw ();
        }

        private void Redraw ()
        {
            // This is Destroying all Action Skill Slots at Runtime
            foreach (Transform child in transform)
            {
                Destroy (child.gameObject);
            }

            // This is Redrawing all available skills at Runtime
            // Will Later set this to total Class skill size most likely

            for (int i = 0; i < skillTree.GetTreeSize (); i++)
            {
                var skillUI = Instantiate (actionSkillPrefab, transform);
                skillUI.Setup (skillTree, i);
            }
            var skillToDraw = skillTree.GetAvailableSkills();
            if (skillToDraw != null)
            {
                Debug.Log("SkillTreeUI found this many skills in GetAvailablrSkills " + skillToDraw.Count);
                Debug.Log("The thing it found was ");
                foreach (var skill in skillToDraw)
                {
                    var icon = Instantiate (skill, transform);
                    skillIcon.SetItem(skill);
                    Debug.Log("set " + skill.name);
                    skillTree.AddToAssignedSlot (skill, 1);

                }

            }
        }
    }
}