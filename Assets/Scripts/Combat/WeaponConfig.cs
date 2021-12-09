﻿using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Pools;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu (fileName = "Weapon", menuName = "Weapons/make New Weapon", order = 0)]
    public class WeaponConfig : EquipableItem, IModifierProvider
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon eqqiuppedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public Weapon Spawn (Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon (rightHand, leftHand);

            Weapon weapon = null;

            if (eqqiuppedPrefab != null)
            {
                Transform handtransform = GetHandTransform (rightHand, leftHand);
                weapon = Instantiate (eqqiuppedPrefab, handtransform);
                weapon.gameObject.name = weaponName;
            }
            GetWeaponOverrideController(animator);

            return weapon;
        }

        public AnimatorOverrideController GetWeaponOverrideController (Animator animator)
        {
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return animatorOverride;
        }

        private void DestroyOldWeapon (Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find (weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find (weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy (oldWeapon.gameObject);
        }

        private Transform GetHandTransform (Transform rightHand, Transform leftHand)
        {
            Transform handtransform;
            if (isRightHanded)
            {
                handtransform = rightHand;
            }
            else handtransform = leftHand;
            return handtransform;
        }

        public bool HasProjectile ()
        {
            return projectile != null;
        }

        public void LaunchProjectile (Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate (projectile, GetHandTransform (rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget (target, instigator, calculatedDamage);
        }

        public float GetWeaponDamage ()
        {
            return weaponDamage;
        }

        public float GetPercentageBonus ()
        {
            return percentageBonus;
        }

        public float GetWeaponRange ()
        {
            return weaponRange;
        }

        public IEnumerable<float> GetAdditiveModifiers (Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return weaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers (Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return percentageBonus;
            }
        }
    }
}