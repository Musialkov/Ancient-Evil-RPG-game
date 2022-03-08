using System.Collections;
using System.Collections.Generic;
using Musialkov.Inventories;
using UnityEngine;
using RPG.Quests;
using RPG.Movement;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        Pickup pickup;
        GameObject player;
        Mover mover;

        int maxDistance = 2;

        private void Awake() 
        {
            pickup = GetComponent<Pickup>();
            player = GameObject.FindGameObjectWithTag("Player");
            mover = player.GetComponent<Mover>();
        }

        public CursorType GetCursorType()
        {
            if(pickup.CanBePickedUp())
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.None;
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0) && ChceckIfDistanceIsSmallEnough())
            {
                pickup.PickupItem();
                CheckIfIsQuestItem();
            }
            else if(Input.GetMouseButtonDown(0) && !ChceckIfDistanceIsSmallEnough())
            {
                if(mover.CanMoveTo(transform.position))
                {
                    mover.StartMoveAction(transform.position);
                }
            }
            return true;
        }  

        private void CheckIfIsQuestItem()
        {
            var questCompletion = GetComponent<QuestCompletion>();
            if(questCompletion != null) 
            {
                questCompletion.CompleteObjective();
            }
        }

        private bool ChceckIfDistanceIsSmallEnough()
        {
            if(Vector3.Distance(transform.position, player.transform.position) < maxDistance) return true;
            else return false;
        }
    } 
}

