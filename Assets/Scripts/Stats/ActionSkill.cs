using System;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Stats
{

    [CreateAssetMenu (menuName = ("RPG/Skills/Action Skill"))]
    public class ActionSkill : ActionItem
    {
        [Header ("Generic Skill Info")]
        [SerializeField] int levelToLearn = 1;
        [SerializeField] PlayerClass skillClass;
        [SerializeField] int skillTreeSlot = 1;
        [SerializeField] float skillCoolDown;

        [Header ("Skill Type")]
        public bool hasActiveTime = false;
        public bool hasCoolDown = false;
        public bool isPoolRegenerating = false;

        [Header ("Buff Skill info")]
        [SerializeField] bool isPermanent = false;
        [SerializeField] float timeActive = 10f;
        [SerializeField]
        Modifier[] additiveModifiers;
        [SerializeField]
        Modifier[] percentageModifiers;

        [SerializeField] Pool poolToRegenerate;
        [SerializeField] float regenerationAmount = 0.1f;
        [SerializeField] float defaultBuffAdditiveBonus = 1f;
        [SerializeField] float defaultBuffPercentageBonus = 1f;

        GameObject player;
        TemporaryBuff temporaryBuff;
        bool thisToggle = false;

        [System.Serializable]
        public struct Modifier
        {
            public Stat stat;
            public float value;
        }

        void Awake ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");

        }

        public override void Use (GameObject user) // Make this more customizable and not just heal
        {
            Debug.Log ("Using action: " + this);

            if (hasActiveTime)
            {
                //temporaryBuff.SetBuffTime (statToBuff, timeActive);
            }

        }


        // public void CastAttackSkill (float skillCooldown, )
        // {
        //     if (!isCastable) return;
        //     this.skillCooldown = skillCooldown;
        //     this.skillRange = skillRange;
        //     this.skillDamage = skillDamage;
        //     this.skillRegeneration = skillRegeneration;
        //     this.isPoolGenerating = isPoolGenerating;
        // }

        public int GetLevelRequired ()
        {
            return levelToLearn;
        }

        public PlayerClass GetSkillClass ()
        {
            return skillClass;
        }

        public float GetSkillRegeneration ()
        {
            return regenerationAmount;
        }

        public float GetSkillCooldown ()
        {
            return skillCoolDown;
        }

        public bool GetisRegenerating ()
        {
            return isPoolRegenerating;
        }

        public int GetSlot ()
        {
            return skillTreeSlot;
        }

        public Modifier[] GetSkillAddModifiers()
        {
            return additiveModifiers;
        }

        public Modifier[] GetSkillPercentModifiers()
        {
            return percentageModifiers;
        }



    }
}