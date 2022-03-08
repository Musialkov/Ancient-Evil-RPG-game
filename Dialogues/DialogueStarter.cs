using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using Musialkov.Saving;

namespace RPG.Dialogues
{
    public class DialogueStarter : MonoBehaviour, ISaveable
    {
        [SerializeField] AIDialogueManager aIDialogueManager;
        [SerializeField] bool deleteAtFirstTalk;
        bool shouldBeDeleted;
        BoxCollider collider;
        PlayerDialogueManager playerDialogueManager;
        GameObject player;
        
        private void Awake()
        {
            collider = GetComponent<BoxCollider>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerDialogueManager = player.GetComponent<PlayerDialogueManager>();
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.tag == "Player" && aIDialogueManager != null)
            {
                aIDialogueManager.StartDialogue(player.GetComponent<PlayerController>());
                if(deleteAtFirstTalk)
                {
                    SwitchOffObject();
                }
            }           
        }

        public void SwitchOffObject()
        {
            shouldBeDeleted = true;
            collider.isTrigger = false;           
        }

        public object CaptureState()
        {
            return shouldBeDeleted;
        }

        public void RestoreState(object state)
        {
            bool delete = (bool) state;
            shouldBeDeleted = delete;
            if(shouldBeDeleted)
            {
                SwitchOffObject();
            }
        }
    }
}