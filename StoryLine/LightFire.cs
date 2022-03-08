using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Movement;
using UnityEngine;
using UnityEngine.VFX;

namespace RPG.StoryLine
{
    public class LightFire : MonoBehaviour, IRaycastable
    {
        public bool IsActive { get; set; }

        [SerializeField] Transform trans;
        [SerializeField] VisualEffect fire;

        Mover mover;
        bool canActivate = false;

        private void Awake()
        {
            IsActive = false;
            mover = GameObject.FindGameObjectWithTag("Player").GetComponent<Mover>();
        }

        private void Update()
        {
            if (mover.NavMeshAgent.destination.z == trans.position.z &&
                mover.NavMeshAgent.destination.x == trans.position.x)
            {
                canActivate = true;
            }
            else canActivate = false;

            float distance = Vector3.Distance(mover.gameObject.transform.position, trans.position);
            if (distance < 0.2 && canActivate && !IsActive)
            {
                fire.Play();
                Debug.Log("Zapalamy światło");
                IsActive = true;
            }
        }
        public CursorType GetCursorType()
        {
            return CursorType.Action;
        }

        public bool HandleRaycast(PlayerController player)
        {
            if (!enabled) return false;
            Mover mover = player.GetComponent<Mover>();
            if (Input.GetMouseButton(0))
            {
                mover.MoveTo(trans.position);
            }
            return true;
        }
    }

}