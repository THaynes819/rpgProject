using UnityEditor;
using UnityEngine;



namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntityMe : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";


        public string GetUniqueIdentifier()
        {
            return "";
        }

        public object CaptureState()
        {
            print("Capturing State for " + GetUniqueIdentifier());
            return null;
        }

        public void RestoreState(object state)
        {
            print("Restoring state for " + GetUniqueIdentifier());
        }

        private void Update() 
        {            
            if (Application.isPlaying) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");            
            
            if(string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
                
            }
                   
        }

    }
}