using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RPG.Combat.SkillTree;

namespace RPG.UI
{
    [RequireComponent (typeof (Image))]
    public class ActionSkillIcon : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] GameObject textContainer = null;
        [SerializeField] TextMeshProUGUI itemNumber = null;

        public void SetItem (ActionSkill actionSkill)
        {
            SetItem (actionSkill, 0);
        }
        public void SetItem (ActionSkill actionSkill, int number)
        {
            var iconImage = GetComponent<Image> ();
            if (actionSkill == null)
            {
                iconImage.enabled = false;
            }
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = actionSkill.GetIcon();
            }

            if (itemNumber)
            {
                if (number <= 1)
                {
                    textContainer.SetActive (false);
                }
                else
                {
                    textContainer.SetActive (true);
                    itemNumber.text = number.ToString ();
                }
            }
        }

    }

}