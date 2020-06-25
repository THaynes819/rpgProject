using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using GameDevTV.UI.Inventories;
using RPG.Combat;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ActionSkillSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] ActionSkillIcon icon = null;

        // STATE
        int index;
        ActionSkill skill;
        SkillTree skillTree;

        void Awake ()
        {

        }

        // PUBLIC

        public void Setup (SkillTree skillTree, int index)
        {
            this.skillTree = skillTree;
            this.index = index;
            icon.SetItem (skillTree.GetSkillInSlot(index)); // Write this Method in SkillTree
        }

        public int MaxAcceptable (InventoryItem skillAsItem)
        {
            return 1;
        }

        public void AddItems (InventoryItem skillAsItem, int number) //Add to Store
        {
            skillTree.AddSkillToSlot (index, skillAsItem as ActionSkill);
        }

        public InventoryItem GetItem () // May need to Cast?
        {
            return skillTree.GetSkillInSlot (index);
        }

        public int GetNumber () // Use Store
        {
            return 1;
        }

        public void RemoveItems (int number) //Remove From Store
        {
            skillTree.RemoveFromSlot (index, number);
        }
    }
}