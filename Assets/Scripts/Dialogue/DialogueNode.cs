using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        bool isPlayer = false;
        [SerializeField]
        string text;
        [SerializeField]
        List<string> children = new List<string> ();
        [SerializeField]
        Rect rect = new Rect (0, 0, 200, 100);
        [SerializeField] string onEnterAction;
        [SerializeField] string onExitAction;
        [SerializeField] Condition condition;
        //[SerializeField] Condition condition2;

        public bool IsPlayerSpeaking ()
        {
            return isPlayer;
        }

        public Rect GetRect ()

        {
            return rect;
        }

        public string GetText ()
        {
            return text;
        }

        public List<string> GetChildren ()
        {
            return children;
        }

        public string GetOnEnterAction ()
        {
            return onEnterAction;
        }

        public string GetOnExitAction ()
        {
            return onExitAction;
        }

        public bool CheckCondition (IEnumerable<IPredicateEvaluator> evaluators)
        {
            bool conditionToCheck = condition.Check(evaluators);
            //bool condition2ToCheck = condition2.Check(evaluators);


            if (conditionToCheck == true)
            {
                return conditionToCheck;
            }

            return false;
        }

//#if UNITY_EDITOR
        public void SetPosition (Vector2 newPosition)
        {
#if UNITY_EDITOR
            Undo.RecordObject (this, "Move Dialogue Node");
            rect.position = newPosition;
            EditorUtility.SetDirty (this);
#endif
        }

        public void SetIsPlayerSpeaking (bool newIsPlayerSpeaking)
        {
#if UNITY_EDITOR
            Undo.RecordObject (this, "Change Dialogue Speaker");
            isPlayer = newIsPlayerSpeaking;
            EditorUtility.SetDirty (this);
#endif
        }

        public void SetText (string newText)
        {
#if UNITY_EDITOR
            if (newText != text)
            {
                Undo.RecordObject (this, "Update Dialogue Text");
                text = newText;
                EditorUtility.SetDirty (this);
            }
#endif
        }

        public void AddChild (string childID)
        {
#if UNITY_EDITOR
            Undo.RecordObject (this, "Add Dialogue Link");
            children.Add (childID);
            EditorUtility.SetDirty (this);
#endif
        }

        public void RemoveChild (string childID)
        {
#if UNITY_EDITOR
            Undo.RecordObject (this, "Remove Dialogue Link");
            children.Remove (childID);
            EditorUtility.SetDirty (this);
#endif
        }
    }
}