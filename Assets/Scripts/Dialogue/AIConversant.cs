using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Control;
using RPG.Movement;
using RPG.Pools;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {

        [SerializeField] Dialogue dialogue = null;
        [SerializeField] Fighter fighter = null;
        [SerializeField] string npcName = null;
        [SerializeField] float speakingDistance = 2f;

        PlayerConversant playerConversant;
        bool isCloseEnough = false;

        void Start ()
        {

        }

        public string GetNPCName ()
        {
            if (npcName == null || npcName == "")
            {
                return "Unknown Person";
            }
            else
            {
                return npcName;
            }
        }

        public CursorType GetCursorType ()
        {
            if (fighter.enabled == true)
            {
                dialogue = null;
                return CursorType.Combat;
            }
            else
            {
                return CursorType.Dialogue;
            }

        }

        public bool HandleRaycast (PlayerController callingController)
        {
            if (dialogue == null)
            {
                return false;
            }

            if (GetComponent<Health>().IsDead()) return false;

            if (Input.GetMouseButtonDown (0) && GetIsCloseEnough())
            {
                callingController.GetComponent<PlayerConversant> ().StartDialogue (this, dialogue);
            }
            if (Input.GetMouseButtonDown (0) && !GetIsCloseEnough())
            {
                callingController.GetComponent<Mover>().MoveTo(transform.position, 1f);
                StartCoroutine(MoveToConverse(callingController));
            }
            return true;
        }

        IEnumerator MoveToConverse(PlayerController callingController)
        {
            yield return new WaitUntil(() => GetIsCloseEnough());
            callingController.GetComponent<PlayerConversant> ().StartDialogue (this, dialogue);
        }

        public bool GetIsCloseEnough()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (Vector3.Distance(player.transform.position, transform.position) < speakingDistance)
            {
                isCloseEnough = true;
            }
            else
            {
                isCloseEnough = false;
            }
            return isCloseEnough;
        }

        public Dialogue GetDialogue()
        {
            return dialogue;
        }
    }
}