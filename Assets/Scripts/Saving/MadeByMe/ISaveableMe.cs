
using UnityEngine;

namespace RPG.Saving
{
    public interface ISaveableMe
    {
        object CaptureState();
        void RestoreState(object state);

    }
}
