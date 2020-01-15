using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
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
}
