using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

namespace RPG.Pools
{
    public class TemporaryBuff : MonoBehaviour
    {


        float buff;

        public IEnumerable SetBuffTime (Stat statToBuff, float timeActive)
        {
            //add buff then remove it after time is up
            yield return buff;
        }

    }

}