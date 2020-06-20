using System.Collections;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public interface ICastable
    {
        IEnumerator CastAttackSpell(Transform rightHand, Transform lefthand, Health target, GameObject instigator );
    }
}