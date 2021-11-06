using System;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu (fileName = "Health Change Effect", menuName = "RPG/Abilities/Effect/Health Change", order = 0)]
    public class HealthChangeEffect : EffectStrategy
    {
        [SerializeField] bool isDamageEffect;
        [SerializeField] float healthChangeAmount = 5f;

        public override void StartEffect (GameObject user, IEnumerable<GameObject> targets, Action finished)
        {

            foreach (var target in targets)
            {
                var health = target.GetComponent<Health> ();
                if (health)
                {
                    if (isDamageEffect)
                    {
                        health.TakeDamage (user, -healthChangeAmount);
                    }
                    else
                    {
                        health.Heal(healthChangeAmount);
                    }

                }
            }
            finished ();
        }
    }
}