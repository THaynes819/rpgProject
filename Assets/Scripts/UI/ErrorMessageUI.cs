using TMPro;
using UnityEngine;

    
    namespace RPG.UI
    {
        public class ErrorMessageUI : MonoBehaviour 
        {
            [SerializeField] string displayText = null;
            [SerializeField] TextMeshProUGUI displayOpject;

            private void DrawError()
            {
                displayOpject.text = displayText;
            }
        }
    }