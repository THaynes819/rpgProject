using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] string[] objectives;

        public string GetQuesttitle()
        {
            return this.name;
        }

        public string[] GetObjectives()
        {
            return objectives;
        }

    }
}