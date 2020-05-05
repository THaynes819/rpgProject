using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace RPG.Saving
{
    public class SavingSystemMe : MonoBehaviour
    {       

        string path;

        public void Save(string saveFile)
        {
            path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            using (FileStream stream =  File.Open(path, FileMode.Create))
            {                
                BinaryFormatter formatter = new BinaryFormatter();                
                formatter.Serialize(stream, CaptureState());
            } 
        }

        public void Load(string saveFile)
        {
            path = GetPathFromSaveFile(saveFile);
            print("Loading from " + GetPathFromSaveFile(saveFile));
            using (FileStream stream = File.Open(path, FileMode.Open))
            {                
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState(formatter.Deserialize(stream));
            }
        }

        private object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach ( SaveableEntityMe saveable in FindObjectsOfType<SaveableEntityMe>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();                
            } 
            return state;
        }

        private void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (SaveableEntityMe saveable in FindObjectsOfType<SaveableEntityMe>())
            {                
                saveable.RestoreState(stateDict[saveable.GetUniqueIdentifier()]);
            }
        }    

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");            
        }       
    } 
}