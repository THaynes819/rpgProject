using GameDevTV.Inventories;
using GameDevTV.UI.Inventories;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UIElements;
namespace RPG.UI
{
    public class ActionSkillSlotUI : MonoBehaviour, IItemHolder
    {
        // CONFIG DATA
        [SerializeField] ActionSkillIcon icon = null;
        [SerializeField] GameObject buttonPrefab = null;

        GameObject player;

        // STATE
        int index = 0;
        ActionSkill actionSkill;
        SkillTree skillTree;
        bool toggle;

        void Start ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");
            skillTree = player.GetComponent<SkillTree> ().GetSkillTree ();
        }

        public void Setup (SkillTree skillTree, int index)
        {
            this.skillTree = skillTree;
            this.index = index;
            if (icon != null)
            {
                icon.SetItem (skillTree.GetSkillInSlot (index), index);
            }
            SetupButton ();
        }

        //Makes ToolTips Work
        public InventoryItem GetItem ()
        {
            return skillTree.GetSkillInSlot (index);
        }

        public int GetIndex ()
        {
            return this.index;
        }

        private void SetupButton ()
        {
            if (buttonPrefab != null)
            {
                var buttonInstance = Instantiate(buttonPrefab);
            //buttonInstance.onClick AddListener (OnButtonClick);
            }
        }

        public void OnButtonClick ()
        {
            toggle = !toggle;
            skillTree.ToggleSkill (index, toggle);
        }
    }
}