using System.Collections.Generic;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using GameDevTV.UI.Inventories;
using RPG.Stats;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static RPG.Stats.SkillTree;

namespace RPG.UI
{
    public class ActionSkillSlotUI : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;
        GameObject player;

        // STATE

        ActionSkill actionSkill;
        SkillTree skillTree;

        void Awake ()
        {

        }

        void Start ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");
            skillTree = player.GetComponent<SkillTree> ();
            Debug.Log ("the skillTree is " + skillTree);
            Setup ();

        }

        public void Setup ()
        {
            Debug.Log ("Setup was called");
            //actionSkill = skillTree.GetSkillBook () [index].item;
            icon.SetItem (actionSkill);

        }

        public void OnSkillSelect ()
        {
            var skillBook = skillTree.GetSkillBook ();
            foreach (var skill in skillBook)
            {
                if (index == skill.GetSlot ())
                {
                    Debug.Log ("Selected " + index + " Which should be the same as this: " + skillTree.GetSkillBook ());
                }
            }

        }

        // public void AddItems (InventoryItem item, int index) //Add to Store  (
        // {
        //     skillTree.StoreInSlot (item, index);
        // }

        // public InventoryItem GetItem () // May need to Cast?
        // {
        //     Debug.Log ("GetItem is should be returning an item: " + actionSkill);
        //     return skillTree.GetSkillinSlot (index);
        // }

        // public int MaxAcceptable (InventoryItem skillAsItem)
        // {
        //     return skillTree.MaxAcceptable (skillAsItem, index);
        // }

        // public int GetNumber () // Use Store
        // {
        //     return 1;
        // }

        // public void RemoveItems (int number) //Remove From Store
        // {
        //     skillTree.RemoveFromSlot (index);
        // }

        // void UpdateIcon ()
        // {

        //     icon.SetItem (GetItem ());
        // }
    }
}