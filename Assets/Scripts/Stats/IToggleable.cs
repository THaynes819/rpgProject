using System.Collections;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Stats
{
    public interface IToggleable
    {
        void OnSkillSelect (int index, bool toggle);
    }
}