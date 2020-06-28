using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Stats
{

    [CreateAssetMenu (menuName = ("RPG/Skills/Action Skill"))]
    public class ActionSkill : ActionItem, IModifierProvider
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
        [SerializeField] float timeActive = 10f;
        [SerializeField] Stat statToBuff = Stat.Health;
        [SerializeField] Pool poolToRegenerate;
        [SerializeField] float regenerationAmount = 0.1f;
        [SerializeField] float buffAdditiveBonus = 1f;
        [SerializeField] float buffPercentageBonus = 1f;

        GameObject player;
        TemporaryBuff temporaryBuff;
        bool isActive = false;

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
            if (!hasActiveTime)
            {
                SetPermanentBuff ();
            }

        }

        public void ToggleSkill (int index, bool toggle)
        {
            Debug.Log ("Action Skill says the index is " + index + " and toggle is " + toggle);
            if (skillTreeSlot == index)
            {
                isActive = toggle;
            }
        }

        private void SetPermanentBuff ()
        {
            if (!isActive) return;
            if (buffAdditiveBonus > 0)
            {
                Debug.Log ("Adding Stat");
                GetAdditiveModifiers (statToBuff);
            }
            if (buffPercentageBonus > 0)
            {
                Debug.Log ("multiplying stat");
                GetPercentageModifiers (statToBuff);
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

        public IEnumerable<float> GetAdditiveModifiers (Stat stat)
        {
            stat = statToBuff;
            yield return buffAdditiveBonus;
        }

        public IEnumerable<float> GetPercentageModifiers (Stat stat)
        {
            stat = statToBuff;
            yield return buffPercentageBonus;
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable ()
        {
            //Won't Woork for wwhat I want
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable ()
        {

        }
    }

}