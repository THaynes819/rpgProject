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
            {
                Debug.Log("It is an over time effect. going to CoRoutine");
                float tickVal = healthChangeAmount / effectTimeSpan;
                Health health = data.GetUser().GetComponent<Health>();
                health.StartCoroutine(Tick(isDamageEffect, effectTimeSpan, tickVal, data, finished));                
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
                        health.Heal(healthChangeAmount);
                    }

                }
            }
            finished ();
        }  
        public IEnumerator Tick(bool isDamageEffect, float effectTimeSpan, float tickValue, AbilityData data, Action finished) //
        {
            Debug.Log("Tick CoRoutine Started");
            float amountOfTicks = effectTimeSpan/tickValue;
            float timer = 1;
            
            for (var i = 0; i < amountOfTicks; i++)
            {
                Debug.Log("Tick Counter Started");
                foreach (var target in data.GetTargets())
                {
                    var health = target.GetComponent<Health> ();
                    if (i < amountOfTicks && health)
                    {
                        Debug.Log("Trying to HoT or DoT"); //TODO Make another CoRoutine to make each tick
                        timer -= Time.deltaTime;
                        if (timer == 0)
                        {
                            Debug.Log("Timer hit 0, applying HoT ot DoT");
                            if (isDamageEffect)
                            {
                                Debug.Log("Damaging a Tick");
                                health.TakeDamage (data.GetUser(), -tickValue);
                            }
                            else
                            {
                                Debug.Log("Healing a Tick");
                                health.Heal(tickValue);
                            }

                        }
                    }
                }
            }
            yield return new WaitForSeconds(effectTimeSpan);
            finished ();
        }      
    }
}
