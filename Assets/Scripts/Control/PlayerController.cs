using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        Health health;
        Fighter fighter;

        enum CursorType
        {
            None,
            Movement,
            Combat
        }

        [System.Serializable]
        struct CursorMapping
        {            
            public CursorType type;
            public Texture2D cursorTexture;
            public Vector2 hotSpot;     
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        
        
        private void Awake() 
        {
            health = GetComponent<Health>();            
        }

        private void Update()
        {
            if (health.IsDead()) { return; }
            if (InteractWithCombat()) { return; }
            if (InteractWithMovement()) { return; }

            SetGameCursor(CursorType.None);
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());            
            foreach (RaycastHit hit in hits)
            {                
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();    
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) 
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                SetGameCursor(CursorType.Combat);
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
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);                    
                }
                SetGameCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private void SetGameCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.cursorTexture, mapping.hotSpot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }


        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}