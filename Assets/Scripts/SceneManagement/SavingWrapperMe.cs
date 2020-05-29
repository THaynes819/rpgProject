using System.Collections;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapperMe : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        float fadeintime = 0.5f;

        private void Awake()
        {            
            StartCoroutine (LoadLastScene());            
        }

        IEnumerator LoadLastScene()
        {            
            yield return GetComponent<SavingSystemMe>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImediate();
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
            GetComponent<SavingSystemMe>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystemMe>().Load(defaultSaveFile);
        }

        public void Delete()
        {
            GetComponent<SavingSystemMe>().Delete(defaultSaveFile);
        }
    }
}