using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {

        [SerializeField] Dialogue dialogue = null;
        [SerializeField] Fighter fighter = null;
        [SerializeField] string npcName = null;

        PlayerConversant playerConversant;

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
            if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1))
            {
                callingController.GetComponent<PlayerConversant> ().StartDialogue (this, dialogue);
            }
            return true;
        }
    }
}