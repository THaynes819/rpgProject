using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{

    [CreateAssetMenu (menuName = ("RPG/Inventory/Stats Action Item"))]
    public class ActionSkill : ActionItem, IModifierProvider
    {

        [SerializeField] float healingAmount = 20f;
        [SerializeField] float damageAmount = 15f;
        [SerializeField] float timeActive = 10f;
        [SerializeField] Stat statToBuff = Stat.Health;
        [SerializeField] float buff = 1f;

        public bool hasActiveTime = false;
        public bool isHealing = false;
        public bool isDamaging = false;

        TemporaryBuff temporaryBuff;

        public override void Use (GameObject user) // Make this more customizable and not just heal
        {
            Debug.Log ("Using action: " + this);
            if ( hasActiveTime)
            {
                temporaryBuff.SetBuffTime(statToBuff, timeActive);
            }
            //var healthPoints = user.GetComponent<Health> ();
            //healthPoints.Heal (healingAmount);
        }



        public IEnumerable<float> GetAdditiveModifiers (Stat stat)
        {
            throw new System.NotImplementedException ();
        }

        public IEnumerable<float> GetPercentageModifiers (Stat stat)
        {
            throw new System.NotImplementedException ();
        }
    }

}