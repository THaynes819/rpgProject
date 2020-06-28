using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RPG.Stats.SkillTree;

namespace RPG.UI
{
	[RequireComponent (typeof (Image))]
	public class ActionSkillIcon : MonoBehaviour
	{
		// CONFIG DATA
		[SerializeField] GameObject textContainer = null;
		[SerializeField] TextMeshProUGUI skillNumber = null;

		SkillTree skillTree;
		Dictionary<int, ActionSkill> allSkills;

		public void SetItem (ActionSkill actionSkill)
		{

			SetItem (actionSkill);
		}
		public void SetItem (ActionSkill actionSkill, int index)
		{
			Debug.Log ("Skill Set Item Called");
			var iconImage = GetComponent<Image> ();
			if (actionSkill == null) // null check is wrong here ptobably
			{
				iconImage.enabled = false;
			}
			else
			{
				Debug.Log ("Icon should be enabled");
				iconImage.enabled = true;
				iconImage.sprite = actionSkill.GetIcon ();
			}

			textContainer.SetActive (false);

		}
	}
}