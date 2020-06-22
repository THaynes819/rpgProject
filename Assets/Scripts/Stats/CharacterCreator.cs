using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class CharacterCreator : MonoBehaviour, IStatsModifier
    {

        PlayerClass playerClass;


        public void SetPlayerClass (int val)
        {
            if (val == 0) // turn into a for loop
            {
                playerClass = PlayerClass.Fighter;
            }
            if (val == 1)
            {
                playerClass = PlayerClass.Caster;
            }
            if (val == 2)
            {
                playerClass = PlayerClass.Archer;
            }
        }

        public PlayerClass GetPlayerClass ()
        {
            return playerClass;
        }
    }
}