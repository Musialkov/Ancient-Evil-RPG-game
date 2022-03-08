using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using RPG.Core;

namespace RPG.Dialogues
{
    [System.Serializable]
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] bool isPlayerSpeaking;
        [SerializeField] string text;
        [SerializeField] List<string> childrenNodes = new List<string>();
        [SerializeField] Rect rect = new Rect (0, 0, 200, 150);
        [SerializeField] string onEntryAction;
        [SerializeField] string onExitAction;
        [SerializeField] Condition condition;

        public bool IsPlayerSpeaking(){ return isPlayerSpeaking; }
        public string GetText() { return text; }
        public List<string> GetChildrenNodes() { return childrenNodes; }
        public Rect GetRect() { return rect; }
        public string GetOnEntryAction() { return onEntryAction; }
        public string GetOnExitAction() { return onExitAction; }

        public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
        {
           return condition.CheckCondition(evaluators);
        }

#if UNITY_EDITOR
        public void SetIsPlayerSpeaking(bool isPlayerSpeaking)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");
            this.isPlayerSpeaking = isPlayerSpeaking;
            EditorUtility.SetDirty(this);
        }
        public void SetText (string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(string newChildID)
        {
            Undo.RecordObject(this, "Add Dialogue Node");
            childrenNodes.Add(newChildID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childIDToRemove)
        {
            Undo.RecordObject(this, "Unlink Dialogue Node");
            childrenNodes.Remove(childIDToRemove);
            EditorUtility.SetDirty(this);
        }

        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}