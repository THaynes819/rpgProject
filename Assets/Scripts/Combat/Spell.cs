using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu (fileName = "Spell", menuName = "RPG/Spell/make New Spell", order = 0)]
    public class Spell : ScriptableObject, ISerializationCallbackReceiver
    {



        [SerializeField] float spellCost = 5f;
        [SerializeField] float spellResourceGeneration = 1f;
        [SerializeField] float castTime = 1f;
        [SerializeField] float spellDamage = 10f;
        [SerializeField] float spellRange = 15f;
        [SerializeField] float spellCoolDown = 1f;
        [SerializeField] string spellID = null;
        [SerializeField] Sprite icon = null;
        [SerializeField] bool isRightHanded;
        [SerializeField] Projectile projectile = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Transform preCastFXLocation = null;

        public Stat resourceType;

        public bool isPoolGenerating = false;

        Health target;


        static Dictionary<string, Spell> spellLookupCache;

        public static Spell GetFromID (string spellID)
        {
            if (spellLookupCache == null)
            {
                spellLookupCache = new Dictionary<string, Spell> ();
                var spellList = Resources.LoadAll<Spell> ("");
                foreach (var spell in spellList)
                {
                    if (spellLookupCache.ContainsKey (spell.spellID))
                    {
                        Debug.LogError (string.Format ("Looks like there's a duplicate GameDevTV.UI.InventorySystem ID for objects: {0} and {1}", spellLookupCache[spell.spellID], spell));
                        continue;
                    }

                    spellLookupCache[spell.spellID] = spell;
                }
            }

            if (spellID == null || !spellLookupCache.ContainsKey (spellID)) return null;
            return spellLookupCache[spellID];
        }

        public Stat GetResourceType()
        {
            return resourceType;
        }

        public float GetSpellCost ()
        {
            return spellCost;
        }

        public bool GesDoesSpellGenerate()
        {
            return isPoolGenerating;
        }

        public float GetSpellResourceGeneration()
        {
            return spellResourceGeneration;
        }

        public float GetCastTime ()
        {
            return castTime;
        }

        public float GetSpellDamage ()
        {
            return spellDamage;
        }

        public float GetSpellRange ()
        {
            return spellRange;
        }

        public float GetSpellCoolDown ()
        {
            return spellCoolDown;
        }

        public bool GetIsRightHanded ()
        {
            return isRightHanded;
        }

        public Projectile GetProjectile ()
        {
            return projectile;
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverride;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize ()
        {
            // Generate and save a new UUID if this is blank.
            if (string.IsNullOrWhiteSpace (spellID))
            {
                spellID = System.Guid.NewGuid ().ToString ();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize ()
        {
            // Require by the ISerializationCallbackReceiver but we don't need
            // to do anything with it.
        }
    }

}