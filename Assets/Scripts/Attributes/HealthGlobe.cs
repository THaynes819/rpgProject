using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthGlobe : MonoBehaviour
    {

        [SerializeField] Slider slider = null;

        GameObject player;

        void Awake ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");
        }

        void Update ()
        {
            slider.value = player.GetComponent<Health> ().GetFraction ();
        }
    }
}