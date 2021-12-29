using System.Collections;
using RPG.Pools;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        Fighter fighter;
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] float respawnTime = 5f;

        private void OnTriggerEnter (Collider other)
        {
            if (other.gameObject == GameObject.FindWithTag ("Player"))
            {
                Pickup (other.gameObject);
            }
        }

        private void Pickup (GameObject subject)
        {
            if (weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon (weapon);
            }
            if (healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore, false, false, 0, 0); 
            }
            StartCoroutine (HideForSeconds (respawnTime));
        }

        private IEnumerator HideForSeconds (float seconds)
        {
            ShowPickup (false);
            yield return new WaitForSeconds (seconds);
            ShowPickup (true);
        }

        private void ShowPickup (bool shouldshow)
        {
            GetComponent<Collider> ().enabled = shouldshow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive (shouldshow);
            }
        }

        public bool HandleRaycast (PlayerController callingController)
        {
            if (Input.GetMouseButtonDown (0))
            {
                Pickup (callingController.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType ()
        {
            return CursorType.WeaponPickup;
        }
    }
}