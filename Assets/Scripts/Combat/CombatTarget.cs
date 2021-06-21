using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent (typeof (Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {

        [SerializeField] Fighter fighter = null;


        public CursorType GetCursorType ()
        {
            if (fighter.enabled)
            {
                return CursorType.Combat;
            }
            else
            {
                return CursorType.Dialogue;
            }

        }

        public bool HandleRaycast (PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter> ().CanAttack (gameObject))
            {
                return false;
            }

            if (Input.GetMouseButtonDown (0))
            {
                callingController.GetComponent<Fighter> ().Attack (gameObject);
            }

            if (Input.GetMouseButtonDown (1))
            {
                callingController.GetComponent<Caster> ().Cast (gameObject);
            }
            return true;
        }

        private void Start ()
        {

        }
    }
}