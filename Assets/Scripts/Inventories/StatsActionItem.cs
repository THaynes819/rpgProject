using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Inventories
{

    [CreateAssetMenu (menuName = ("RPG/Inventory/Stats Action Item"))]
    public class StatsActionItem : ActionItem
    {

        [SerializeField] float healingAmount = 20f;

        public override void Use (GameObject user) // Make this more customizable and not just heal
        {

            var healthPoints = user.GetComponent<Health> ();
            healthPoints.Heal (healingAmount);

        }
    }

}