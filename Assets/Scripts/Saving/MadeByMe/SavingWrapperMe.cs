using System.Collections;
using UnityEngine;
using RPG.Saving;


namespace RPG.SceneManagement
{
    public class SavingWrapperMe : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        float fadeintime = 0.5f;

        IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeintime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }

}