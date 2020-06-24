using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using RPG.Combat;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ActionSkillSlotUI : MonoBehaviour //, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] ActionSkillIcon icon = null;

        // STATE
        int index;
        ActionSkill skill;
        SkillTree skillTree;
        ActionStore store;

        void Awake ()
        {
            store = GameObject.FindGameObjectWithTag ("Player").GetComponent<ActionStore> ();
        }

        // PUBLIC

        public void Setup (SkillTree skillTree, int index)
        {
            this.skillTree = skillTree;
            this.index = index;
            //icon.SetItem (skillTree.GetItemInSlot (index));
        }

        public int MaxAcceptable (ActionSkill skill)
        {
            return store.MaxAcceptable (skill, index);
        }

        public void AddItems (ActionSkill actionSkill, int number) //Add to Store
        {
            store.AddAction (actionSkill, index, number);
        }

        public InventoryItem GetItem () // May need to Cast?
        {
            return store.GetAction (index);
        }

        public int GetNumber () // Use Store
        {
            return store.GetNumber (index);
        }

        public void RemoveItems (int number) //Remove From Store
        {
            store.RemoveItems (index, number);
        }
    }
}