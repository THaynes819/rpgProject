using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;
using RPG.Movement;
using System;
using System.Collections;

namespace RPG.Control
{
    [RequireComponent (typeof (RPGPickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        RPGPickup pickup;
        bool withinDistance = false;

        PlayerController playerController;

        private void Awake ()
        {
            pickup = GetComponent<RPGPickup> ();
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        public CursorType GetCursorType ()
        {   
            if (pickup.CanBePickedUp () && GetWithinDistance())
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.FullPickup;
            }
        }

        public bool HandleRaycast (PlayerController callingController)
        {
            float distance = Vector3.Distance(playerController.transform.position, pickup.transform.position);
            bool withinDistance = distance < playerController.GetLootDistance();
            if (Input.GetMouseButtonDown (0) && GetWithinDistance())
            {
                pickup.PickupItem ();
            }
            if (Input.GetMouseButtonDown (0) && !GetWithinDistance())
            {
                playerController.gameObject.GetComponent<Mover>().MoveTo(transform.position, 1f);
                StartCoroutine(MoveToLoot());
            }
            return true;
        }

        IEnumerator MoveToLoot()
        {
            yield return new WaitUntil(() => GetWithinDistance());
            pickup.PickupItem ();
        }

        private bool GetWithinDistance()
        {
            float distance = Vector3.Distance(playerController.transform.position, pickup.transform.position);
            withinDistance = distance < playerController.GetLootDistance();

            return withinDistance;
        }
    }
}