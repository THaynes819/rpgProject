using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Camera playerCamera;

    // Update is called once per frame
    void Update()
    {
        playerCamera = Camera.main;
        if (Input.GetButton("Fire1"))
        {                      
            MoveToCursor();
        }
    }
    private void MoveToCursor()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            GetComponent<Mover>().MoveTo(hit.point);
        }
    }
}
