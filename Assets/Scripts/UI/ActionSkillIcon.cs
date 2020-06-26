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
		Image iconImage;
		SkillTree skillTree;
		Dictionary<int, ActionSkill> allSkills;

		public void SetItem (ActionSkill actionSkill)
		{
			SetItem (actionSkill);
		}
		public void SetItem (ActionSkill actionSkill, int index)
		{
			if (actionSkill == null)
			{
				iconImage.enabled = false;
			}
			else
			{
				Debug.Log("Icon should be enabled");
				iconImage.enabled = true;
				iconImage.sprite = actionSkill.GetIcon ();
			}

			textContainer.SetActive (false);

		}
	}
}