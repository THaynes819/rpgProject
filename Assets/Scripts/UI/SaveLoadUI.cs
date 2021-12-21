using RPG.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SaveLoadUI : MonoBehaviour 
    {
        [SerializeField] Transform contentRoot = null;
        [SerializeField] GameObject layoutButtonPrefab = null;
        SavingWrapper savingWrapper;

        private void OnEnable() //Load Menu needs to start off disabled for this to have the desired effect
        {
            savingWrapper = FindObjectOfType<SavingWrapper>();
            if (savingWrapper == null) return;
            
            foreach (Transform child in contentRoot)
            {
                Destroy(child.gameObject);
            }
            SpawnButtons();
        }

        private void SpawnButtons() // Populates the load Game menu with a button for each Save File that exists
        {
            if (savingWrapper == null)
            {
                Debug.Log("Save/LoadUI.SpawnButtons can't find the Saving Wrapper");
                return;
            }

            foreach (string save in savingWrapper.ListSaves())
            {
                if (!string.IsNullOrEmpty(save))
                {
                    GameObject buttonInstance = Instantiate(layoutButtonPrefab, contentRoot);
                    TMP_Text instanceText = buttonInstance.GetComponentInChildren<TMP_Text>();
                    instanceText.text = save;
                    Button button = buttonInstance.GetComponentInChildren<Button>();
                    button.onClick.AddListener(() => 
                    {
                        savingWrapper.LoadGame(save);
                    });

                }
            }
        }

        public void CreateNewSave(string saveFile) // possibly to add a single save in the future
        {
            Instantiate(layoutButtonPrefab, transform);
        }

        public void LoadGame(string saveFile)
        {

        }
    }
}