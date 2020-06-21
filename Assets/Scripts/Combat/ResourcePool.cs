using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class ResourcePool : MonoBehaviour
    {
        [SerializeField] float regenerationRate = 0.1f;
        [SerializeField] bool doesRegeneratePassively = true;

        [SerializeField] Pool[] resourcePools = null;

        LazyValue<float> resourcePoints;

        void Awake ()
        {
            resourcePoints = new LazyValue<float> (GetInitialPool);
        }

        private float GetInitialPool ()
        {
            // foreach (var pool in resourcePools)
            // {
            //     if (Spell.res) Select Class in UI - Class will select the resource and so on...
            // }
            return GetComponent<BaseStats> ().GetPool (Pool.Mana);
        }

        void Start ()
        {
            resourcePoints.ForceInit ();
        }

        void Update ()
        {

            RegenerateResource ();
        }

        public float GetCurrentResourcePoints ()
        {
            return resourcePoints.value;
        }

        public bool IsRegenerating ()
        {
            if (!doesRegeneratePassively) return false;
            return true;
        }

        public float GetPoolFraction ()
        {
            return resourcePoints.value / GetComponent<BaseStats> ().GetPool (Pool.Mana);
        }

        private void RegenerateResource ()
        {
            float maxPool = GetComponent<BaseStats> ().GetPool (Pool.Mana);
            if (IsRegenerating () && resourcePoints.value < maxPool)
            {
                resourcePoints.value += (regenerationRate * Time.deltaTime);
            }
        }
    }
}