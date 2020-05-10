using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject eqqiuppedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 10f;

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
       
        public void Spawn(Transform handtransform, Animator animator)
        {
            if(eqqiuppedPrefab != null)
            {
            Instantiate(eqqiuppedPrefab, handtransform);
            }
            
            if(animatorOverride != null)
            {
            animator.runtimeAnimatorController = animatorOverride;
            }
        }
    }
}