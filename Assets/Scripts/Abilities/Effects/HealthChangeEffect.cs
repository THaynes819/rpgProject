using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Pools;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu (fileName = "Health Change Effect", menuName = "RPG/Abilities/Effect/Health Change", order = 0)]
    public class HealthChangeEffect : EffectStrategy
    {
        [SerializeField] bool isDamageEffect;        
        [SerializeField] bool isOverTimeEffect = false;

        [Tooltip("Non smooth heals/Damage will tick each second at the fastest. Smooth Effects change health smoothly")]
        [SerializeField] bool isSmoothEffect = false;

        [Tooltip("Duration is seconds for Non Smooth. Smooth Effects are currently frame dependent")]
        [SerializeField] float effectDuration;

        [Tooltip("Damaging effects should be negative numbers. Test to ensure")]
        [SerializeField] float healthChangeAmount = 5;

        [Tooltip("100 appears to be the sweet spot")]
        [SerializeField] float tickSpeed = 100;

        public override void StartEffect (AbilityData data, Action finished)
        {
            //Debug.Log("Health Effect Initiated");
            if (!isOverTimeEffect)
            {   //Debug.Log("It is an Immediate effect");
                ImediateEffect(data, finished);
            }
            if (isOverTimeEffect)
                OverTimeEffect(data, finished);
        }

        private void OverTimeEffect(AbilityData data, Action finished)
        {       
            Health targetHealth = data.GetUser().GetComponent<Health>();   
            foreach (var target in data.GetTargets())
            {
                if (isDamageEffect) // need to make a DOT effect similar to HOT effect in Health Scipt
                {
                    Debug.Log("Damaging a Tick");
                    targetHealth = target.GetComponent<Health>();                        
                    Tick(isDamageEffect, data, targetHealth, healthChangeAmount, finished);
                }
                else
                {
                    HealOverTime(data, targetHealth, healthChangeAmount, finished );
                }
                
            }
            finished ();
        }

        private void HealOverTime(AbilityData data, Health targetHealth, float healthChangeAmount, Action finished)
        {
            isDamageEffect = false; 
            if (isSmoothEffect)
            {
                targetHealth.Heal(healthChangeAmount, true, true, effectDuration, tickSpeed);
            }
            else
            {
                Tick(isDamageEffect, data, targetHealth, healthChangeAmount, finished);
            }
            
            
        }

        private void ImediateEffect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                var health = target.GetComponent<Health> ();
                if (health)
                {
                    if (isDamageEffect)
                    {
                        health.TakeDamage (data.GetUser(), -healthChangeAmount);
                    }
                    else
                    {
                        health.Heal(healthChangeAmount, false, false, 0, 0);
                    }

                }
            }
            finished ();
        }  
        public void Tick(bool isDamageEffect, AbilityData data, Health health, float healthChangeAmount, Action finished) //
        {
            if (isDamageEffect)
            {
                float damagedHealth = health.GetHealthPoints() - healthChangeAmount;
                health.TakeDamage(data.GetUser(), Mathf.Lerp(health.GetHealthPoints(), damagedHealth, tickSpeed));
            }

            if (!isDamageEffect)
            {
                float healedHealth = health.GetHealthPoints() + healthChangeAmount;
                health.Heal(healthChangeAmount, true, false, effectDuration, tickSpeed); 
            }
            
            finished ();
        }
    }      
}

