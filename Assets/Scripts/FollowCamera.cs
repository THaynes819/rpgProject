using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
   [SerializeField] Transform target;

    Camera playerCamera;
    
    void Update()
    {
        transform.position = target.position;
    }
}
