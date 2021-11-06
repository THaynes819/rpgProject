using System;
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        Health health;
        Fighter fighter;
        GameObject player;
        SkillTree skillTree;
        ActionStore actionStore;

            [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D cursorTexture;
            public Vector2 hotSpot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float raycastMaxDistance = 1;
        [SerializeField] float rayCastradius = 1;
        [SerializeField] int numberOfAbilities = 6;

        bool isDraggingUI = false;

        private void Awake ()
        {
            health = GetComponent<Health> ();
            player = GameObject.FindGameObjectWithTag ("Player");
            skillTree = player.GetComponent<SkillTree> ();
            actionStore = GetComponent<ActionStore> ();
        }

        private void Update ()
        {
            if (InteractWithUI ())
            {
                SetGameCursor (CursorType.UI);
                return;
            }
            if (health.IsDead ())
            {
                SetGameCursor (CursorType.None);
                return;
            }

            UseAbilities ();

            if (InteractWithComponent ()) { return; }
            if (InteractWithMovement ()) { return; }

            SetGameCursor (CursorType.None);
        }

        public void OnSkillSelect (int index)
        {
            Debug.Log ("called from" + index);
        }

        private bool InteractWithUI ()
        {
            if (Input.GetMouseButtonUp (0))
            {
                isDraggingUI = false;
            }
            if (EventSystem.current.IsPointerOverGameObject ())
            {
                if (Input.GetMouseButtonDown (0))
                {
                    isDraggingUI = true;
                }
                SetGameCursor (CursorType.UI); //gameobject means UI gameobject
                return true;
            }
            if (isDraggingUI)
            {
                return true;
            }
            return false;
        }

        private void UseAbilities ()
        {
            for (int i = 0; i < numberOfAbilities; i++)
            {
                if (Input.GetKeyDown (KeyCode.Alpha1 + i))
                {
                    actionStore.Use (i, gameObject);
                }
            }
        }

        private bool InteractWithComponent ()
        {
            RaycastHit[] hits = RaycastAllSorted ();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable> ();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast (this))
                    {
                        SetGameCursor (raycastable.GetCursorType ());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted ()
        {
            RaycastHit[] hits = Physics.SphereCastAll (GetMouseRay (), rayCastradius);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort (distances, hits);
            return hits;
        }

        private bool InteractWithMovement ()
        {

            Vector3 target;
            bool hasHit = RaycsatNavmesh (out target);
            if (hasHit)
            {
                if (!GetComponent<Mover> ().CanMoveTo (target)) return false;

                if (Input.GetMouseButton (0))
                {
                    GetComponent<Mover> ().StartMoveAction (target, 1f);
                }
                SetGameCursor (CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycsatNavmesh (out Vector3 target)
        {
            target = new Vector3 ();

            RaycastHit hit;
            bool hasHit = Physics.Raycast (GetMouseRay (), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToMesh = NavMesh.SamplePosition (hit.point, out navMeshHit, raycastMaxDistance, NavMesh.AllAreas);
            if (!hasCastToMesh) return false;

            target = navMeshHit.position;

            return true;
        }

        private void SetGameCursor (CursorType type)
        {
            CursorMapping mapping = GetCursorMapping (type);
            Cursor.SetCursor (mapping.cursorTexture, mapping.hotSpot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping (CursorType type)
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

        public static Ray GetMouseRay ()
        {
            return Camera.main.ScreenPointToRay (Input.mousePosition);
        }
    }
}