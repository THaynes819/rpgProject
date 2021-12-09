using System.Collections;
using RPG.Pools;
using UnityEngine;

namespace RPG.Stats
{
    public interface IToggleable
    {
        void OnSkillSelect (int index, bool toggle);
    }
}