// using System.Collections;
// using System.Collections.Generic;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.UI.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.UI
{
    public class SkillTreeUI : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] ActionSkillSlotUI skillSlotPrefab = null;

        // CACHE
        SkillTree playerSkillTree;

        void Awake ()
        {

        }
        private void Start ()
        {
            playerSkillTree = GameObject.FindGameObjectWithTag ("Player").GetComponent<SkillTree> ().GetSkillTree ();
            playerSkillTree.skillTreeUpdated += Redraw;
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

            for (int i = 0; i < playerSkillTree.GetTreeSize (); i++)
            {
                var skillUI = Instantiate (skillSlotPrefab, transform);
                skillUI.Setup (playerSkillTree, i);
            }
        }
    }
}