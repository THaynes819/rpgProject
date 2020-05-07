using System.Collections;
using UnityEngine;
using RPG.Saving;


namespace RPG.SceneManagement
{
    public class SavingWrapperMe : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        float fadeintime = 0.5f;

        // IEnumerator Start()
        // {
        //     Fader fader = FindObjectOfType<Fader>();
        //     fader.FadeOutImediate();
        //     yield return GetComponent<SavingSystemMe>().LoadLastScene(defaultSaveFile);
        //     yield return fader.FadeIn(fadeintime);
        // }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                Save();
            }
        }

        private void Save()
        {
            GetComponent<SavingSystemMe>().Save(defaultSaveFile);
        }

        private void Load()
        {
            GetComponent<SavingSystemMe>().Load(defaultSaveFile);
        }
    }

}