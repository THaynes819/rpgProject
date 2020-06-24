using System.Collections;
using System.Collections.Generic;
using GameDevTV.UI.Inventories;
using RPG.Combat;
using UnityEngine;

namespace RPG.UI
{
    public class ActionSkillUI : MonoBehaviour //IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] ActionSkillSlotUI actionSkillPrefab = null;

        // CACHE
        ActionSkill actionSkill;
        SkillTree skillTree;

        void Awake ()
        {
            skillTree = SkillTree.GetPlayerSkillTree ();
        }

        // private void Redraw ()
        // {
        //     foreach (Transform child in transform)
        //     {
        //         Destroy (child.gameObject);
        //     }

        //     for (int i = 0; i < skillTree.GetSize (); i++)
        //     {
        //         var skillUI = Instantiate (actionSkillPrefab, transform);
        //         skillUI.Setup (skillTree, i);
        //     }
        // }
    }
}