using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using System;
using RPG.Combat;


namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        Camera playerCamera;
        Combat.Fighter attack;
        Fighter fighter;
        
        void Update()
        {
            
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            print("Nothing To Do");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            
            foreach (RaycastHit hit in hits)
            {
                fighter = GetComponent<Fighter>();
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();    
                if (target == null) continue;  
                                         
                if (!fighter.CanAttack(target.gameObject)) continue;                
                if (Input.GetButton("Fire1"))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetButton("Fire1"))
                {
                    GetComponent<Mover>().MoveTo(hit.point);                    
                }
                return true;
            }
            return false;
        }
            
       

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}