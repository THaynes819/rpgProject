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

        Stat currenResourcePool;
        PlayerClass playerClass;

        LazyValue<float> resourcePoints;

        void Awake ()
        {
            resourcePoints = new LazyValue<float> (GetInitialResource);
        }

        private float GetInitialResource ()
        {
            return GetComponent<BaseStats> ().GetStat (currenResourcePool);
        }

        void Start ()
        {
            InitialPool ();
            resourcePoints.ForceInit ();
        }

        public float SetCurrentResourcePoints(float pointsChange)
        {
            resourcePoints.value += pointsChange;
            return resourcePoints.value;
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
            return resourcePoints.value / GetComponent<BaseStats> ().GetStat (currenResourcePool);
        }

        private Stat InitialPool ()
        {
            playerClass = GetComponent<BaseStats>().GetPlayerClass();
            if (playerClass == PlayerClass.Fighter)
            {
                currenResourcePool = Stat.Rage;
                return currenResourcePool;
            }
            if (playerClass == PlayerClass.Caster)
            {
                currenResourcePool = Stat.Mana;
                return currenResourcePool;
            }
            if (playerClass == PlayerClass.Archer)
            {
                currenResourcePool = Stat.Fixation;
                return currenResourcePool;
            }
            currenResourcePool = Stat.None;

            return currenResourcePool;

        }

        void Update ()
        {
            InitialPool (); // Remove after Character Selection Proccess is Complete!!!
            GetCurrentPool(); // Also to be removed
            RegenerateResource ();
            //Debug.Log ("Current Resource Pool is " + currenResourcePool);
            //Debug.Log ("Current Resource Amount is " + resourcePoints.value + " " + currenResourcePool);
        }

        public Stat GetCurrentPool ()
        {
            currenResourcePool = InitialPool ();
            //Debug.Log ("GetCurrentPool says the current Pool is " + currenResourcePool);
            return currenResourcePool;
        }

        private void RegenerateResource ()
        {
            float maxPool = GetComponent<BaseStats> ().GetStat (currenResourcePool);
            if (IsRegenerating () && resourcePoints.value < maxPool)
            {
                resourcePoints.value += (regenerationRate * Time.deltaTime);
            }
        }
    }
}