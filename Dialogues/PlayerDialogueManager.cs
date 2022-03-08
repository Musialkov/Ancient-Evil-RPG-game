using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using RPG.Control;
using UnityEngine;

namespace RPG.Dialogues
{
    public class PlayerDialogueManager : MonoBehaviour
    {
        [SerializeField] string playerName = "Ricky";

        AIDialogueManager aiDialogueManager = null;
        Dialogue dialogue = null;
        DialogueNode currentNode = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;

        public void StartDialogue (AIDialogueManager newConversant, Dialogue newDialogue) 
        {
            Debug.Log(newConversant);
            Debug.Log(newDialogue);
            aiDialogueManager = newConversant;
            dialogue = newDialogue;
            currentNode = dialogue.GetRootNode();
            this.GetComponent<PlayerController>().SwitchPlayerControll(false);
            onEnterActionTrigger();
            onConversationUpdated();       
        }

        public void QuitDialogue()
        {
            onExitActionTrigger();
            aiDialogueManager = null;
            dialogue = null;
            currentNode = null;
            isChoosing = false;
            this.GetComponent<PlayerController>().SwitchPlayerControll(true);
            onConversationUpdated();
        }

        public bool IsActive()
        {
            return dialogue != null;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public void SelectChoice(DialogueNode node) 
        {
            currentNode = node;
            isChoosing = false;
            onEnterActionTrigger();
            Next();
        }

        public string GetCurrentConversantName()
        {
            if(isChoosing) return playerName;
            return aiDialogueManager.GetNPCName();
        }

        public string GetText()
        {
            if (currentNode == null) {return "";}
            return currentNode.GetText();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return FilterOnCondition(dialogue.GetPlayerChildren(currentNode));
        }

        public void Next()
        {
            int numPlayerResponses = FilterOnCondition(dialogue.GetPlayerChildren(currentNode)).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                onExitActionTrigger();
                onConversationUpdated();
                return;
            }
            DialogueNode[] childNodes = FilterOnCondition(dialogue.GetAIChildren(currentNode)).ToArray();
            onExitActionTrigger();
            currentNode = childNodes[0];
            onEnterActionTrigger();

            onConversationUpdated();
        }

        public bool HasNext()
        {
            DialogueNode[] childNodes = FilterOnCondition(dialogue.GetChildren(currentNode)).ToArray();
            if (childNodes.Length == 0) { return false; }
            return true;
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> dialogueNode) 
        {
            foreach (var node in dialogueNode)
            {
                if(node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }

        private void  onEnterActionTrigger()
        {
            if(currentNode != null)
            {
                TriggerAction(currentNode.GetOnEntryAction());
            }
        }

        private void onExitActionTrigger()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        private void TriggerAction(string action)
        {
            if (action == "") {return;}
            foreach (var trigger in aiDialogueManager.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }
    }
}
