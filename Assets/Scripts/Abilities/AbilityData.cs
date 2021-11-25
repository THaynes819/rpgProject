using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

public class AbilityData : IAction
{
GameObject user;
Vector3 targetedPoint;
IEnumerable<GameObject> targets;
bool isCancelled = false;

    public AbilityData(GameObject user)
    {
        this.user = user;
    }

    public IEnumerable<GameObject> GetTargets()
    {
        return targets;
    }    

    public void SetTargets(IEnumerable<GameObject> newTargets)
    {
        this.targets = newTargets;
    }

    public Vector3 GetTargetedPoint()
    {
        return targetedPoint;
    }

    public void SetTargetedPoint(Vector3 newTargetedPoint)
    {
        this.targetedPoint = newTargetedPoint;
    }

    public GameObject GetUser()
    {
        return user;
    }

    public void StartCoroutine(IEnumerator coroutine)
    {
        user.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
    }

    public void Cancel()
    {
        isCancelled = true;
    }

    public bool GetIsCancelled()
    {
        return isCancelled;
    }
}