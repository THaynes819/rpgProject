using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour

{
    
    Camera playerCamera;    
    NavMeshAgent agent;
   
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
        agent = GetComponent<NavMeshAgent>();

        if(hasHit)
        {
            agent.destination = hit.point;
        }
    }    
}
