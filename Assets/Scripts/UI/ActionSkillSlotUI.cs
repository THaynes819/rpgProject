using GameDevTV.Inventories;
using GameDevTV.UI.Inventories;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

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
        bool toggle;
        [SerializeField] Button buttonPrefab;

        void Start ()
        {
            //player = GameObject.FindGameObjectWithTag ("Player");
            //skillTree = player.GetComponent<SkillTree> ().GetSkillTree ();

            buttonPrefab = GetComponent<Button> ();
        }

        public void Setup (SkillTree skillTree, int index)
        {
            this.skillTree = skillTree;
            this.index = index;
            icon.SetItem (skillTree.GetSkillInSlot (index), 1);
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
            var buttonInstance = GetComponent<Button> ();
            buttonInstance = Instantiate (buttonPrefab, transform);
            buttonInstance.onClick.AddListener (OnButtonClick);
        }

        public void OnButtonClick ()
        {
            toggle = !toggle;
            Debug.Log ("button was cklicked. Sending from this index: " + index);
            skillTree.ToggleSkill (index, toggle);
        }
    }
}