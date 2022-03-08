using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Movement;

namespace RPG.Dialogues
{
    public class AIDialogueManager : MonoBehaviour, IRaycastable
{
    [SerializeField] string npcName = "Bob";
    [SerializeField] Dialogue dialogue = null;
    [SerializeField] Transform dialoguePosition;


    public string GetNPCName()
    {
        return npcName;
    }

    public Dialogue GetDialogue()
    {
        return dialogue;
    }

    public CursorType GetCursorType()
    {
        return CursorType.Dialogue;
    }

    public void RemoveDialogue()
    {
        dialogue = null;
    }

    public void StartDialogue(PlayerController player)
    {
        player.GetComponent<Mover>().MoveTo(dialoguePosition.position);
        player.GetComponent<PlayerDialogueManager>().StartDialogue(this, dialogue);
    }

    public bool HandleRaycast(PlayerController player)
    {
        if (dialogue == null) { return false; }
        if(Input.GetMouseButtonDown(0))
        {
            StartDialogue(player);
        }
        return true;
    }
}

}