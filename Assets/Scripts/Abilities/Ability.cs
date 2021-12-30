using GameDevTV.Inventories;
using RPG.Stats;
using RPG.Pools;
using UnityEngine;
using RPG.Core;

namespace RPG.Abilities
{
    [CreateAssetMenu (fileName = "New Ability", menuName = "RPG/Abilities/Ability", order = 0)]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;
        [SerializeField] Stat abiltyResource;
        [SerializeField] float resourceCost = 5;
        [SerializeField] bool doesCancelActions = true;

        public override void Use (GameObject user) // Make this more customizable and not just heal
        {   
            float currentResourcePoints = user.GetComponent<ResourcePool>().GetCurrentResourcePoints();
            if (resourceCost > currentResourcePoints)
            {
                //add a not enough mana effect?
                return;
            }

            CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
            if (cooldownStore.GetTimeRemaining(this) > 0)
            {
                //add an On CD effect?              
                return;
            }

            AbilityData data = new AbilityData(user);
            data.SetDoesCancel(doesCancelActions);

            ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(data);

            targetingStrategy.StartTargeting (data, 
                () => {
                    TargetAquired(data);
                });            
        }

        private void TargetAquired (AbilityData data)
        {   
            if (data.GetIsCancelled()) return; // This is having backward effect. Walking doesn't cancel drinking, but drinking still cancels walking.

            float currentResourcePoints = data.GetUser().GetComponent<ResourcePool>().GetCurrentResourcePoints();
            if (resourceCost > currentResourcePoints)
            {
                //add a not enough mana effect?
                return;
            }

            TargetEffect (data);
        }

        private void TargetEffect (AbilityData data)
        {
            var resource = data.GetUser().GetComponent<ResourcePool>();
            if (!resource.UseResource(resourceCost)) return;

            data.GetUser().GetComponent<CooldownStore>().StartCooldown(this, this.GetItemCooldown());
            
            foreach (var filterStrategy in filterStrategies)
            {
                data.SetTargets(filterStrategy.Filter (data.GetTargets()));
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect (data, EffectFinished);
            }
        }

        private void EffectFinished ()
        {
            
        }
    }
}