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

        //[SerializeField] GameObject buttonPrefab = null; //Uneccessary

        [SerializeField] ActionSkillIcon icon = null; 
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
            //Debug.Log("setup called from " + skillTree.gameObject.name);
            this.skillTree = skillTree;
            this.index = index;
            if (icon != null && skillTree != null)
            {
                if (skillTree.GetSkillInSlot (index) != null)
                {
                    //Debug.Log("Setting the Icon for " + skillTree.GetSkillInSlot (index).GetDisplayName() );
                    icon.SetItem (skillTree.GetSkillInSlot (index), index);
                }
                else
                {
                    //Debug.Log("skillTree.GetSkillInSlot() returned null");
                }
            
            }
            //SetupButton ();
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

        // private void SetupButton ()
        // {
        //     Debug.Log("SetupButton Called");
        //     // Likely no need to instantiate at all. Do need to setup icon.
        //     // GameObject buttonInstance = null;
        //     // if (buttonPrefab != null && buttonInstance == null)
        //     // {
        //     //     buttonInstance = Instantiate(buttonPrefab, this.transform);
        //     // //buttonInstance.onClick AddListener (OnButtonClick);
        //     // }
        // }

        public void OnButtonClick ()
        {
            toggle = !toggle;
            skillTree.ToggleSkill (index, toggle);
        }
    }
}