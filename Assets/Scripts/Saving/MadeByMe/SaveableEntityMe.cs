using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntityMe : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";


        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            print("Capturing State for " + GetUniqueIdentifier());
            return new SerializableVector3Me(transform.position);
        }

        public void RestoreState(object state)
        {
            print("Restoring state for " + GetUniqueIdentifier());
            SerializableVector3Me position = (SerializableVector3Me)state;
            
            transform.position = position.ToVectorMe();
            GetComponent<NavMeshAgent>().Warp(position.ToVectorMe());
            GetComponent<ActionScheduler>().CancelCurrentACtion();        
        }

#if UNITY_EDITOR
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
#endif
    }
}