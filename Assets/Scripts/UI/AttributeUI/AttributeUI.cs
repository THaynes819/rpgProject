using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class AttributeUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI uncommitedPointText;
        [SerializeField] Button confirmButton;

        AttributeStore playerAttributeStore = null;        

        private void Start() 
        {
            playerAttributeStore = GameObject.FindGameObjectWithTag ("Player").GetComponent<AttributeStore>();
            confirmButton.onClick.AddListener(playerAttributeStore.Commit);
        }

        private void Update()
        {
            uncommitedPointText.text = playerAttributeStore.GetUnassignedPoints().ToString();
        }
    }
}