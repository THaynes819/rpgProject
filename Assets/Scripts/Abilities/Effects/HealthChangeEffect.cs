using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu (fileName = "Health Change Effect", menuName = "RPG/Abilities/Effect/Health Change", order = 0)]
    public class HealthChangeEffect : EffectStrategy
    {
        [SerializeField] bool isDamageEffect;        
        [SerializeField] bool isOverTimeEffect = false;
        [SerializeField] float effectTimeSpan;
        [SerializeField] float healthChangeAmount = 5f;

        public override void StartEffect (AbilityData data, Action finished)
        {
            Debug.Log("Health Effect Initiated");
            if (!isOverTimeEffect)
            {   Debug.Log("It is an Immediate effect");
                ImediateEffect(data, finished);
            }
            if (isOverTimeEffect)
                OverTimeEffect(data, finished);
        }

        private void OverTimeEffect(AbilityData data, Action finished)
        {       
            Health targetHealth = data.GetUser().GetComponent<Health>();   
            float tickVal = healthChangeAmount / effectTimeSpan;            

            for (var i = 0; i < effectTimeSpan; i++)
            {
                foreach (var target in data.GetTargets())
                {
                    Debug.Log("Trying to HoT or DoT. i = " + i + " amount of ticks is " + effectTimeSpan + " the tick value is " + tickVal + " The Effect Time Span is " + effectTimeSpan);

                    if (isDamageEffect)
                    {
                        Debug.Log("Damaging a Tick");
                        targetHealth = target.GetComponent<Health>();
                        bool isHealing = false;
                        
                        targetHealth.StartCoroutine(Tick(isDamageEffect, effectTimeSpan, data, targetHealth, isHealing, tickVal, finished));
                    }
                    else
                    {
                        targetHealth = data.GetUser().GetComponent<Health>();
                        bool ishealing = true;                            
                        targetHealth.StartCoroutine(Tick(isDamageEffect, effectTimeSpan, data, targetHealth, ishealing, tickVal, finished));
                    }
                    
                }
            }
            finished ();
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
                        health.Heal(healthChangeAmount);
                    }

                }
            }
            finished ();
        }  
        public IEnumerator Tick(bool isDamageEffect, float effectTimeSpan, AbilityData data, Health health, bool isHealing, float tickVal, Action finished) //
        {
            Debug.Log("Tick CoRoutine Started");
            float tickTime = 1; // TODO add a haste stat from the character to increase the tick speed        
            for (var i = 0; i < effectTimeSpan; i++)
            {
                if (i == 0)
                {
                    if (isHealing)
                    {
                        health.Heal(tickVal);
                    }
                    if (!isHealing)
                    {
                        health.TakeDamage(data.GetUser(), tickVal);
                    }
                }
                else
                {
                    yield return new WaitForSeconds(tickTime);
                    if (isHealing)
                    {
                        health.Heal(tickVal);                        
                    }
                    if (!isHealing)
                    {
                        health.TakeDamage(data.GetUser(), tickVal);
                    }
                }
            }
            
            
            
        }      
    }
}
