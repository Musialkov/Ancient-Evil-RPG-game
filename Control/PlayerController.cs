using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Fighter fighter;
        Health health;
        Mover mover;

        void Start() 
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            if(health.IsDead()) return;
            if(InteractWithCombat()) return;
            if(InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] raycastHits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit raycastHit in raycastHits)
            {
                CombatTarget target = raycastHit.transform.GetComponent<CombatTarget>();
                
                if(target == null || !fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    fighter.Attack(target.gameObject);                   
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out raycastHit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(raycastHit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
