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
    public class ActionSkillSlotUI : MonoBehaviour, IItemHolder
    {
        // CONFIG DATA
        [SerializeField] ActionSkillIcon icon = null;

        GameObject player;

        // STATE
        int index = 0;
        ActionSkill actionSkill;
        SkillTree skillTree;

        void Start ()
        {
            //player = GameObject.FindGameObjectWithTag ("Player");
            //skillTree = player.GetComponent<SkillTree> ().GetSkillTree ();
            Debug.Log ("the skillTree is " + skillTree);

        }

        public void Setup (SkillTree skillTree, int index)
        {
            Debug.Log ("Setup was called");
            this.skillTree = skillTree;
            this.index = index;
            icon.SetItem (skillTree.GetSkillInSlot (index), 1);
            Debug.Log ("Setup is setting this Icon " + icon);

        }

        //Makes ToolTips Work
        public InventoryItem GetItem ()
        {
            return skillTree.GetSkillInSlot (index);
        }


    }
}