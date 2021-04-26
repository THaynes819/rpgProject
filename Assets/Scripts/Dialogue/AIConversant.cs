﻿using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {

        [SerializeField] Dialogue dialogue = null;

        PlayerConversant playerConversant;

        public CursorType GetCursorType ()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast (PlayerController callingController)
        {
            if (dialogue == null)
            {
                return false;
            }
            if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1))
            {
                callingController.GetComponent<PlayerConversant>().StartDialogue(dialogue);
            }
            return true;
        }

    }

}