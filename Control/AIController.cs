using UnityEngine.AI;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 10f;
        [SerializeField] float timeOfBeingSuspicious = 3f;
        [SerializeField] float timeOfBeaingAtWaypoint = 2f;
        [SerializeField] float timeOfBeaingAttacking = 10f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float shoutDistance = 5f;
        [SerializeField] PathController pathController;
        
        GameObject player;
        Fighter fighter;
        Health health;
        Mover mover;
        ActionSheduler actionSheduler;

        //Guard memory
        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeOfCurrentBeaingAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            actionSheduler = GetComponent<ActionSheduler>();          
        }

        private void Start() 
        {
            guardPosition = transform.position;
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (fighter.isActiveAndEnabled && ChceckPossibleAttack() && fighter.CanAttack(player))
            {    
                timeSinceLastSawPlayer = 0;
                AttcackBehaviour();
            }
            else if (timeOfBeingSuspicious > timeSinceLastSawPlayer)
            {
                SuscpicousBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            IncreaseTimeVariable();
        }

        private bool ChceckPossibleAttack()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance ||
            fighter.getTimeFromLastAttack() < timeOfBeaingAttacking;
        }

        private void AttcackBehaviour()
        {
            fighter.Attack(player);
        }

        public void AlarmNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai != null && ai != gameObject.GetComponent<AIController>())
                {
                    ai.GetComponent<Fighter>().resetTimeFromLastAttack();
                }
            }
        }

        private void SuscpicousBehaviour()
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            actionSheduler.CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if(pathController != null)
            {
                if(AtWaypoint())
                {
                    timeOfCurrentBeaingAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();            
            }
            if(timeOfCurrentBeaingAtWaypoint > timeOfBeaingAtWaypoint)
            {
                mover.StartMoveAction(nextPosition);
            }        
        }

        private void IncreaseTimeVariable()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeOfCurrentBeaingAtWaypoint += Time.deltaTime;
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = pathController.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return pathController.GetWaypoint(currentWaypointIndex);
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}