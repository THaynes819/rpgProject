using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent (typeof (RPGPickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        RPGPickup pickup;

        private void Awake ()
        {
            pickup = GetComponent<RPGPickup> ();
        }

        public CursorType GetCursorType ()
        {
            if (pickup.CanBePickedUp ())
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
            if (Input.GetMouseButtonDown (0))
            {
                pickup.PickupItem ();
            }
            return true;
        }
    }
}