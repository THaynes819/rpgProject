using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {

        [SerializeField] Dialogue dialogue;

        PlayerConversant playerConversant;

        public CursorType GetCursorType ()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast (PlayerController callingController)
        {
            if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1))
            {
                playerConversant = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerConversant> ();
                playerConversant.StartDialogue(dialogue);
            }
            return true;
        }

    }

}