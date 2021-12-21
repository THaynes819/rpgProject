using System.Collections;
using UnityEngine;
using GameDevTV.Saving;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string currentSaveKey = "currentSaveName";
        [SerializeField] float fadeintime = 0.5f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] int levelOneScene = 1;
        [SerializeField] int mainMenuScene = 0;

        //List<string> allSaveFiles = new List<string>();
        public void ContinueGame()
        {
            if (!PlayerPrefs.HasKey(currentSaveKey)) return;
            if (!GetComponent<SavingSystem>().SaveFileExists(GetCurrentSave())) return;
            StartCoroutine (LoadLastScene());
        }

        public void CreateNewGame(string saveFile)
        {
            Debug.Log("CreateNewGame Called");
            if (String.IsNullOrEmpty(saveFile)) return; //Create New doesn't save in level one until first save after starting. Need to fix. Maybe fixed.
            SetCurrentSave(saveFile);
                
            StartCoroutine(LoadScene (saveFile, levelOneScene));
        }

            public void LoadMenu()
        {
            StartCoroutine(LoadMenuScene());
        }

        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            ContinueGame();
        }

        private void SetCurrentSave(string saveFile)
        {  
            Debug.Log("SetCurrentSave set to " + saveFile);              
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        public string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }
        
        public int GetMainMenuScene()
        {
            return mainMenuScene;
        }

        private IEnumerator LoadMenuScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);            
            yield return SceneManager.LoadSceneAsync (mainMenuScene);
            yield return fader.FadeIn(fadeintime);
        }

        private IEnumerator LoadScene (string saveFile, int scene)
        {
            Debug.Log("Loading Scene " + scene.ToString());
            Fader fader = FindObjectOfType<Fader>();            
            yield return fader.FadeOut(fadeOutTime);  
            yield return SceneManager.LoadSceneAsync (scene);
            GetComponent<SavingSystem>().Save(saveFile);
            yield return fader.FadeIn(fadeintime);
            
        }

        IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());      
            
            yield return fader.FadeIn(fadeintime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Delete();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
        }

        public IEnumerable<string> ListSaves()
        {
            if (GetComponent<SavingSystem>().ListSaves() != null)
            {
                return GetComponent<SavingSystem>().ListSaves();
            }
            else
            {
                Debug.Log("Saving Wrapper Reports List of Saves as null");
                return null;
            }
            
        }
    }
}