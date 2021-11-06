using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu (fileName = "Demo Targeting", menuName = "RPG/Abilities/Targeting/Demo", order = 0)]
    public class DemoTargeting : TargetingStrategy
    {
        public override void StartTargeting (GameObject user, Action<IEnumerable<GameObject>> finished)
        {
            Debug.Log ("Demo Target Started");
            finished(null);
        }

    }
}