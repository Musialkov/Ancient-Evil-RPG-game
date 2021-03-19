using UnityEngine.AI;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 10f;
        [SerializeField] float timeOfBeingSuspicious = 3f;
        [SerializeField] float timeOfBeaingAtWaypoint = 2f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] PathController pathController;
        
        GameObject player;
        Fighter fighter;
        Health health;
        Mover mover;
        ActionSheduler actionSheduler;

        //Guard memory
        Vector3 guardPosition;
       [SerializeField] float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeOfCurrentBeaingAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            actionSheduler = GetComponent<ActionSheduler>();

            guardPosition = transform.position;
        }

        void Update()
        {
            if (health.IsDead()) return;

            if (IsPlayerInChaseRange() && fighter.CanAttack(player))
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

        private void IncreaseTimeVariable()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeOfCurrentBeaingAtWaypoint += Time.deltaTime;
        }

        private bool IsPlayerInChaseRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }

        private void AttcackBehaviour()
        {
            fighter.Attack(player);
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