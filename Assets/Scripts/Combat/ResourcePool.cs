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

        LazyValue<float> resourcePoints;

        void Awake ()
        {

            resourcePoints = new LazyValue<float> (GetInitialPool);
        }

        private float GetInitialPool ()
        {
            return GetComponent<BaseStats> ().GetPool (Pool.Mana); //TODO Set pool through Character class selection or UI or something later
        }

        void Start ()
        {
            resourcePoints.ForceInit ();

            //Debug.Log ("The pool amount is " + resourcePoints.value);
        }

        void Update ()
        {
            // Debug.Log ("The mana fraction is " + GetPoolFraction ());
            // Debug.Log ("The pool amount is " + resourcePoints.value);
            // if (Input.GetMouseButtonDown(0))
            // {
            //     resourcePoints.value -= 1;
            //     doesRegeneratePassively = false;
            // }
            if (Input.GetMouseButtonDown (1))
            {
                doesRegeneratePassively = true;
            }
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