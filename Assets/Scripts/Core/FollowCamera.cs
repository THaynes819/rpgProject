using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
   [SerializeField] Transform target;

    Camera playerCamera;
    
    void LateUpdate()
    {
        {
            transform.position = target.position;
        }
    }    
}
