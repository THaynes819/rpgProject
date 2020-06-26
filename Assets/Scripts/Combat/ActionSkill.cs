using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{

    [CreateAssetMenu (menuName = ("RPG/Skills/Action Skill"))]
    public class ActionSkill : ActionItem, IModifierProvider
    {
        [SerializeField] int levelToLearn = 1;
        [SerializeField] PlayerClass skillClass;
        [SerializeField] float healingAmount = 20f;
        [SerializeField] float skillDamage = 15f;
        [SerializeField] float skillRange = 10f;
        [SerializeField] float skillCost = 5f;
        [SerializeField] float skillRegeneration = 5f;
        [SerializeField] float skillCooldown = 5f;
        [SerializeField] float timeActive = 10f;
        [SerializeField] Stat statToBuff = Stat.Health;
        [SerializeField] float buff = 1f;
        [SerializeField] Health targetToDamage = null;
        [SerializeField] int skillTreeSlot = 1;

        public bool hasActiveTime = false;
        public bool isHealing = false;
        public bool isDamaging = false;
        public bool isPoolGenerating = false;

        GameObject player;
        TemporaryBuff temporaryBuff;

        void Awake ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");
        }

        public override void Use (GameObject user) // Make this more customizable and not just heal
        {
            Debug.Log ("Using action: " + this);
            if (hasActiveTime)
            {
                temporaryBuff.SetBuffTime (statToBuff, timeActive);
            }
            if (healingAmount > 0)
            {
                var healthPoints = user.GetComponent<Health> ();
                healthPoints.Heal (healingAmount);
            }
            if (skillDamage > 0)
            {
                DamageSkill ();
            }

        }

        private void DamageSkill ()
        {
            if (targetToDamage == null)
            {
                player.GetComponent<Caster> ().HandleSkill (this);
            }
        }

        public int GetLevelRequired ()
        {
            return levelToLearn;
        }

        public PlayerClass GetSkillClass()
        {
            return skillClass;
        }

        public float GetSkillRange ()
        {
            return skillRange;
        }

        public float GetSkillDamage ()
        {
            return skillDamage;
        }

        public float GetSkillCost ()
        {
            return skillCost;
        }

        public float GetSkillRegeneration ()
        {
            return skillRegeneration;
        }

        public float GetSkillCooldown ()
        {
            return skillCooldown;
        }

        public bool GetisGenerating ()
        {
            return isPoolGenerating;
        }

        public int GetSlot ()
        {
            return skillTreeSlot;
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