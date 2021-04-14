using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDiaologue;


        public string GetText()
        {
            return currentDiaologue.GetRootNode().GetText();
        }
    }
}