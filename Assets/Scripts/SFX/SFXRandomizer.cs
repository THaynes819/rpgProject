using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SFX
{
    public class SFXRandomizer : MonoBehaviour
    {
        [SerializeField] AudioSource[] sources = null;

        public void PlayRandom()
        {
            int choice = Random.Range(0, sources.Length);
            AudioSource chosenEffect = sources[choice];
            chosenEffect.Play();
        }
    }
}

