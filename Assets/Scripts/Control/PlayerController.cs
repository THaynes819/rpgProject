using System;
using System.Linq;
using RPG.Combat;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        Health health;
        Fighter fighter;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D cursorTexture;
            public Vector2 hotSpot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float raycastMaxDistance = 1.0f;
        [SerializeField] float maxPathLength = 40f;

        private void Awake ()
        {
            health = GetComponent<Health> ();
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

            if (InteractWithComponent ()) { return; }
            if (InteractWithMovement ()) { return; }

            SetGameCursor (CursorType.None);
        }

        private bool InteractWithUI ()
        {
            return EventSystem.current.IsPointerOverGameObject (); //gameobject means UI gameobject
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
            RaycastHit[] hits = Physics.RaycastAll (GetMouseRay ());
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

            NavMeshPath path = new NavMeshPath ();
            bool hasPath = NavMesh.CalculatePath (transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength (path) > maxPathLength) return false;

            return true;

        }

        private float GetPathLength (NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2f) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance (path.corners[i], path.corners[i + 1]);
            }

            return total;
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

        private static Ray GetMouseRay ()
        {
            return Camera.main.ScreenPointToRay (Input.mousePosition);
        }
    }
}