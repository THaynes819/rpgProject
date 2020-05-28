using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        Fighter fighter;
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;       

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == GameObject.FindWithTag("Player"))
            { 
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));                         
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {            
            ShowPickup(false);                      
            yield return new WaitForSeconds(seconds);            
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldshow)
        {            
            GetComponent<Collider>().enabled = shouldshow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldshow);
            }
        }
    }
}