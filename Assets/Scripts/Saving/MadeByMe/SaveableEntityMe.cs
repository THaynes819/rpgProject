using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntityMe : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        static Dictionary<string, SaveableEntityMe> globalLookup = new Dictionary<string, SaveableEntityMe>();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveableMe saveable in GetComponents<ISaveableMe>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;            
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach ( ISaveableMe saveable in GetComponents<ISaveableMe>()) 
            {
               string typeString = saveable.GetType().ToString();
               if(stateDict.ContainsKey(typeString))
               {
                   saveable.RestoreState(stateDict[typeString]);
               }                
            }                   
        }

#if UNITY_EDITOR
        private void Update() 
        {            
            if (Application.isPlaying) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");            
            
            if(string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();                
            }
            globalLookup[property.stringValue] = this;              
        }
#endif
        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate))
            {
                return true;
            }
            
            if (globalLookup[candidate] == this)
            {
                return true;
            }

            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
            }

            return false;
        }
    }
}