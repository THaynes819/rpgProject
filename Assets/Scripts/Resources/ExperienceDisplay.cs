﻿using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;

        private void Awake() 
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }
        
        void Update()
        {
            GetComponent<Text>().text = string.Format("{0:0}", experience.GetExperiencePoints());
        }
    }
}
