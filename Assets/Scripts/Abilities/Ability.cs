using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu (fileName = "New Ability", menuName = "RPG/Abilities/Ability", order = 0)]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;

        public override void Use (GameObject user) // Make this more customizable and not just heal
        {
            Debug.Log ("Using action: " + this);
            targetingStrategy.StartTargeting (user,
                (IEnumerable<GameObject> targets) => {
                    TargetAquired(user, targets);
                });
        }

        private void TargetAquired (GameObject user, IEnumerable<GameObject> targets)
        {
            TargetEffect (user, targets);
        }

        private void TargetEffect (GameObject user, IEnumerable<GameObject> targets)
        {
            foreach (var filterStrategy in filterStrategies)
            {
                targets = filterStrategy.Filter (targets);
            }

            foreach (var target in targets)
            {
                Debug.Log ("The target is " + target.name);
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect (user, targets, EffectFinished);
            }
        }

        private void EffectFinished ()
        {

        }
    }
}