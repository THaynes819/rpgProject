using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]
    public class SerializableVector3Me
    {
        float x, y, z;

        public SerializableVector3Me(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
        
        public Vector3 ToVectorMe()
        {
            
            return new Vector3(x, y, z);
        }
    }
}