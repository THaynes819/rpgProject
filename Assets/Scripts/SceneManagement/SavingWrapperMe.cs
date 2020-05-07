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
            yield return GetComponent<SavingSystemMe>().LoadLastScene(defaultSaveFile);
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
        }

        public void Save()
        {
            GetComponent<SavingSystemMe>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystemMe>().Load(defaultSaveFile);
        }
    }

}