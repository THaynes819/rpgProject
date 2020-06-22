using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    [CreateAssetMenu ( menuName = "RPG/Skill")]
    public class SkillTree : ScriptableObject // make Abstract?
    {

        bool canLearn = false;
        int playerLevel;

        //onLevelUp check to see if high enough level was attained to learn skill
        // Create a Dictionary of Skills ... Probably need to make skills soon if not first
        // Build Array of this players Class Skills


        void Start()
        {
            playerLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>().GetLevel();
        }
    }
}