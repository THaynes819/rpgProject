using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.UI
{
    public interface IPauseEnabler
    {
        void TogglePauseAvailability(bool value);
        bool GetPauseAvailability();
        void ClosePauseNow();
    }
}
