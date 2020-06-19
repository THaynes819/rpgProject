using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthGlobe : MonoBehaviour
    {

        [SerializeField] Slider slider = null;
        [SerializeField] Health player = null;

        void Awake ()
        {

        }

        // Update is called once per frame
        void Update ()
        {
            slider.value = player.GetFraction();
        }
    }
}