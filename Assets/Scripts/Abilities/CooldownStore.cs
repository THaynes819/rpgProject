using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Abilities
{

    public class CooldownStore : MonoBehaviour 
    {
        Dictionary<InventoryItem, float> coolDownTimers = new Dictionary<InventoryItem, float>();

        Dictionary<InventoryItem, float> initialCooldownTimes = new Dictionary<InventoryItem, float>();

        void Start()
        {
            GameObject player = GameObject.FindWithTag("Player");
        }
    

        void Update() 
        {
            var key = new List<InventoryItem>(coolDownTimers.Keys);
            foreach (InventoryItem ability in key)
            {
                coolDownTimers[ability] -= Time.deltaTime;
                if (coolDownTimers[ability] <= 0)
                {
                    coolDownTimers.Remove(ability);
                    initialCooldownTimes.Remove(ability);
                }
            }
        }

        public void StartCooldown(InventoryItem ability, float cooldownTime)
        {
            coolDownTimers[ability] = cooldownTime;
            initialCooldownTimes[ability] = cooldownTime;
        }

        public float GetTimeRemaining(InventoryItem inventoryitem)
        {
            if (!coolDownTimers.ContainsKey(inventoryitem))
            {
                return 0;
            }
            
            return coolDownTimers[inventoryitem];            
        }

        public float GetFractionRemaining(InventoryItem ability)
        {
            if (ability == null)
            {
                return 0;
            }

            if (!initialCooldownTimes.ContainsKey(ability))
            {
                return 0;
            }

            return coolDownTimers[ability] / initialCooldownTimes[ability];
        }
    }
}
