using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Musialkov.Inventories;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Fighter fighter;
        Health health;
        Mover mover;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;  
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxdistanceNavMesh = 1f;
        [SerializeField] float cursorRadius = 1f;

        bool movementStarted = false;
        bool isDraggingUi = false;

        void Awake() 
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            if(health.IsDead()) return;
            CheckSpecialAbilityKeys();

            if (Input.GetMouseButtonUp(0))
            {
                movementStarted = false;
            }

            if (InteractWithUI()) return;
            if(InteractWithComponent()) return;
            if(InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        private void CheckSpecialAbilityKeys()
        {
            var actionStore = GetComponent<ActionStore>();
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                actionStore.Use(0, gameObject);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                actionStore.Use(1, gameObject);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                actionStore.Use(2, gameObject);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                actionStore.Use(3, gameObject);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                actionStore.Use(4, gameObject);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                actionStore.Use(5, gameObject);
            }
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDraggingUi = false;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDraggingUi = true;
                }

                SetCursor(CursorType.None);
                return true;
            }

            if(isDraggingUi)
            {
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] raycastHits = SortRaycastHitByDistance();
            foreach (RaycastHit raycastHit in raycastHits)
            {
                IRaycastable[] raycastables = raycastHit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] SortRaycastHitByDistance()
        {
            RaycastHit[] raycastHits = Physics.SphereCastAll(GetMouseRay(), cursorRadius);
            float[] distances = new float[raycastHits.Length];
            for (int i = 0; i < raycastHits.Length; i++)
            {
             distances[i] = raycastHits[i].distance;   
            }
            Array.Sort(distances, raycastHits);
            return raycastHits;
        }

        private bool InteractWithMovement()
        {         
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {   
                if(!mover.CanMoveTo(target)) return false;

                if (Input.GetMouseButtonDown(0))
                {
                    movementStarted = true;
                }

                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(target);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out raycastHit);
            if(!hasHit) return false;
            
            NavMeshHit navMeshHit;
            bool hasCatToNavMesh = NavMesh.SamplePosition(
                raycastHit.point, out navMeshHit, maxdistanceNavMesh, NavMesh.AllAreas);
            if(!hasCatToNavMesh) return false;

            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if(!hasPath) return false;
            if(path.status != NavMeshPathStatus.PathComplete) return false;
            
            return mover.CanMoveTo(target);
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping cursorMapping = GetCursorMapping(type);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping cursorMapping in cursorMappings)
            {   
                if(cursorMapping.type == type)
                {
                    return cursorMapping;
                }
            }
            return cursorMappings[0];
        }

        public void SwitchPlayerControll(bool turnOn)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = turnOn;
        }
    }
}
