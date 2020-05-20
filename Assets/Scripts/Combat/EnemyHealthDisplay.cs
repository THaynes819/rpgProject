using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;


namespace RPG.Combat
{

    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;           

        private void Awake() 
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update() 
        {
            Health target = fighter.GetTarget();
            
            if(target == null)
            {
                GetComponent<Text>().text = "None";                                
            }
            else if (target.IsDead())
            {
                GetComponent<Text>().text = "Dead";
            }
            else
            {                
                GetComponent<Text>().text = string.Format("{0:0}%", target.GetPercentage());
            }         
        }
    }
}