using System.Collections.Generic;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	[RequireComponent (typeof (Image))]
	public class ActionSkillIcon : MonoBehaviour
	{
		// CONFIG DATA
		//[SerializeField] GameObject textContainer = null;
		[SerializeField] TextMeshProUGUI skillName = null;
		[SerializeField] Image iconImage = null;
		[SerializeField] int skillFontSize = 150;

		SkillTree skillTree;
		Dictionary<int, ActionSkill> allSkills;
		TextMeshProUGUI skillText;

		Sprite icon;

		public void SetItem (ActionSkill actionSkill)
		{
			SetItem (actionSkill);
		}

		public void SetItem (ActionSkill actionSkill, int index)
		{

			if (actionSkill != null) // null check may be unecesarry
			{
				iconImage.sprite = actionSkill.GetIcon ();
				iconImage.enabled = true;
				ChangeSkillFontText (actionSkill);
			}
		}

		private void ChangeSkillFontText (ActionSkill actionSkill)
		{
			if (actionSkill != null)
			{
				skillName.text = actionSkill.GetDisplayName ();
				skillName.isRightToLeftText = false;
				skillName.fontSize = skillFontSize;
				skillName.verticalAlignment = VerticalAlignmentOptions.Middle;
			}
			else
			{
				iconImage.enabled = false;
				Debug.Log ("Check to see if a skill Icon is missing");
			}
		}
	}
}