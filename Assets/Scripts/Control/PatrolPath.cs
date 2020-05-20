using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour 
    {
        [SerializeField] float waypointRadius = 0.3f;

        private void OnDrawGizmos() 
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextindex(i);
                Gizmos.DrawSphere(GetWaypoint(i), waypointRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextindex(int i)
        {            
            if (i + 1 == transform.childCount)
           {
                return 0;
           }
           return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}    

    

