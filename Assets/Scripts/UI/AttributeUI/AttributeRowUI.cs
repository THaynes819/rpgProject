using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class AttributeRowUI : MonoBehaviour 
    {
        [SerializeField] Attribute attribute;
        [SerializeField] TextMeshProUGUI valueText;
        [SerializeField] Button minusButton;
        [SerializeField] Button plusButton;
        AttributeStore playerAttributeStore = null;

        private void Start() 
        {
            playerAttributeStore = GameObject.FindGameObjectWithTag ("Player").GetComponent<AttributeStore>();
            minusButton.onClick.AddListener(() => Allocate(-1));
            plusButton.onClick.AddListener(() => Allocate(1));
        }

        private void Update() 
        {
            minusButton.interactable = playerAttributeStore.CanAssignPoints(attribute, -1);
            plusButton.interactable = playerAttributeStore.CanAssignPoints(attribute, 1);
            
            valueText.text = playerAttributeStore.GetPoints(attribute).ToString();
        }

        public void Allocate(int points)
        {
            playerAttributeStore.AssignPoints(attribute, points);
        }

        
    }
}