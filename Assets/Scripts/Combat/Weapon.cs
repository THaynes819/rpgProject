using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {

        [SerializeField] UnityEvent onHit;
        [SerializeField] AudioSource[] sources = null;

        //Randomize the sound effect
        public void OnHit()
        {
            onHit.Invoke();
        }

        public void PlaySFX()
        {
            int choice = Random.Range(0, sources.Length);
            AudioSource play = sources[choice];
            play.Play();
        }
    }
}