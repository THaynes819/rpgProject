using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public interface IDeathAnnouncer
    {
        void DeathAnnounce(string questName, string objective);
    }
}