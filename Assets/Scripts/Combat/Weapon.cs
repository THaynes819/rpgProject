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
        [SerializeField] bool isRightHanded = true;

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
       
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if(eqqiuppedPrefab != null)
            {
                Transform handtransform;
                if (isRightHanded)
                {
                    handtransform = rightHand;
                }
                else handtransform = leftHand;
                Instantiate(eqqiuppedPrefab, handtransform);
            }
            
            if(animatorOverride != null)
            {
            animator.runtimeAnimatorController = animatorOverride;
            }
        }
    }
}