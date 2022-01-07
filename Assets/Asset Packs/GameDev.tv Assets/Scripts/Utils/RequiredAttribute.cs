using UnityEngine;

namespace GameDevTV.Utils
{
    [System.Serializable]
    public class RequiredAttribute
    {
        [SerializeField] RPG.Stats.Attribute attribute;
        [SerializeField] int requiredValue = 5;

        public RPG.Stats.Attribute GetAttribute()
        {
            return attribute;
        }

        public int GetRequiredValue()
        {
            return requiredValue;
        }
    }

}