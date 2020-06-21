using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{

    public class CharacterCreator : MonoBehaviour
    {

        [SerializeField] CharacterClasses[] characterClasses = null;
        [SerializeField] TMP_Dropdown dropdown = null;

        CharacterClasses characterClass;

        void Update ()
        {
            //var options = dropdown.value;

            Debug.Log("The dropdown value is " + dropdown.value);

            // foreach (var option in options)
            // {
            //     Debug.Log ("The options are " + option);
            // }
        }

    }
}