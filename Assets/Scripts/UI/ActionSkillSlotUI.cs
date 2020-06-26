using System.Collections.Generic;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using GameDevTV.UI.Inventories;
using RPG.Combat;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static RPG.Combat.SkillTree;

namespace RPG.UI
{
    public class ActionSkillSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;
        List<ActionSkill> skillBook;

        // STATE

        ActionSkill actionSkill;
        SkillTree skillTree;

        void Awake ()
        {
            skillTree = GameObject.FindGameObjectWithTag ("Player").GetComponent<SkillTree> ();
            //skillBook = skillTree.GetSkillBook ();
            skillTree.skillTreeUpdated += UpdateIcon;

        }

        void Start ()
        {
            Setup (skillTree, index);
        }

        public void Setup (SkillTree skillTree, int index)
        {
            Debug.Log ("Setup was called");

            this.skillTree = skillTree;
            this.index = index;
            icon.SetItem (skillTree.GetSkillinSlot (index), skillTree.GetNumberInSlot (index));

        }

        public void AddItems (InventoryItem item, int index) //Add to Store  (
        {
            skillTree.StoreInSlot (item, index);
        }

        public InventoryItem GetItem () // May need to Cast?
        {
            Debug.Log ("GetItem is should be returning an item: " + actionSkill);
            return skillTree.GetSkillinSlot (index);
        }

        public int MaxAcceptable (InventoryItem skillAsItem)
        {
            return skillTree.MaxAcceptable (skillAsItem, index);
        }

        public int GetNumber () // Use Store
        {
            return 1;
        }

        public void RemoveItems (int number) //Remove From Store
        {
            skillTree.RemoveFromSlot (index);
        }

        void UpdateIcon ()
        {

            icon.SetItem (GetItem ());
        }
    }
}