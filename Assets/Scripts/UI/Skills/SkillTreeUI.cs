// using System.Collections;
// using System.Collections.Generic;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.UI.Inventories;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SkillTreeUI : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] ActionSkillSlotUI skillSlotPrefab = null;

        // CACHE
        SkillTree playerSkillTree;

        private void Start ()
        {
            GameObject player = GameObject.FindGameObjectWithTag ("Player");
            playerSkillTree = player.GetComponent<SkillTree> ();
        }

        private void OnEnable() 
        {
            if (playerSkillTree == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag ("Player");
                playerSkillTree = player.GetComponent<SkillTree> ();
                //playerSkillTree.skillTreeUpdated += Redraw;  // Posswibly re-add for levelign up
            }
            
            Redraw ();
        }

        private void Redraw ()
        {
            if (playerSkillTree == null)
            {
                Debug.Log("Redraw says Skill Tree is null");
            }
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