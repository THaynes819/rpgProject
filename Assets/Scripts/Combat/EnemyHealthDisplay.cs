using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;


namespace RPG.Combat
{

    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;    
        [SerializeField] bool isDisplayedAsPercent = true;       

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
                return;                             
            }
            else if (target.IsDead())
            {
                GetComponent<Text>().text = "Dead";
            }
            else
            {   
                if (isDisplayedAsPercent)
                {             
                    GetComponent<Text>().text = string.Format("{0:0}%", target.GetPercentage());
                }
                else
                {
                    GetComponent<Text>().text = string.Format("{0:0}/{1:0}", target.GetHealthPoints(), target.GetMaxHealthPoints());
                }
            }         
        }
    }
}