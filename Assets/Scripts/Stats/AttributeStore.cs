using System.Collections.Generic;
using UnityEngine;


namespace RPG.Stats
{
    public class AttributeStore : MonoBehaviour 
    {
        Dictionary<Attribute, int> assignedPoints = new Dictionary<Attribute, int>();

        int unassignedPoints = 10;

        public int GetPoints(Attribute attribue)
        {
            // if the dictionary contains the attribute, then return the assigned points for the attribute. else return 0.
            return assignedPoints.ContainsKey(attribue) ? assignedPoints[attribue] : 0;
        }

        public void AssignPoints(Attribute attribute, int points)
        {
            if (!CanAssignPoints(attribute, points)) return;
            assignedPoints[attribute] = GetPoints(attribute) + points;
            unassignedPoints -= points;
        }

        public bool CanAssignPoints(Attribute attribute, int points)
        {
            if (GetPoints(attribute) + points < 0) return false;
            if (unassignedPoints < points ) return false;
            return true;
        }

        public int GetUnassignedPoints()
        {
            return unassignedPoints;
        }
    }
}