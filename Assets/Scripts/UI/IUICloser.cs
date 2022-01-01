using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public interface IUICloser
    {
        void CloseAll();

        string GetGameObjectName();

        bool GetIsActive();
    }
}


