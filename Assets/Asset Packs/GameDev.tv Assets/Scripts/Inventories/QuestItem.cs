using GameDevTV.Utils;
using UnityEngine;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// An inventory item that can be placed in the action bar and "Used".
    /// </summary>
    /// <remarks>
    /// This class should be used as a base. Subclasses must implement the `Use`
    /// method.
    /// </remarks>
    [CreateAssetMenu (menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Quest Item"))]
    public class QuestItem : InventoryItem
    {
        // CONFIG DATA
        [Tooltip ("Does an instance of this item get consumed every time it's used.")]
        [SerializeField] bool consumable = false;
        [SerializeField] bool isQuestItem = true;
        [SerializeField] string questName;
        [SerializeField] string questObjective;
        [SerializeField] Condition condition;

        // PUBLIC

        /// <summary>
        /// Trigger the use of this item. Override to provide functionality.
        /// </summary>
        /// <param name="user">The character that is using this action.</param>
        public virtual void Use (GameObject user)
        {
            Debug.Log ("Using action: " + this);

        }

        public bool isConsumable ()
        {
            return consumable;
        }

        public bool IsQuestItem ()
        {
            return isQuestItem;
        }

        public string GetItemObjective()
        {
            return questObjective;
        }

        // private void completeItemObjective ()
        // {
        //     var player = GameObject.FindWithTag ("Player");
        //     player.GetComponent<IPredicateEvaluator>()
        // }
    }
}