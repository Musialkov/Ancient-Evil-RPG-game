using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using Musialkov.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxLengthNavPath = 40f;
        
        public NavMeshAgent NavMeshAgent {get; set;}
        Health health;
        ActionSheduler actionSheduler;
        Animator animator;

        private void Awake() 
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            actionSheduler = GetComponent<ActionSheduler>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if(health.IsDead())
            {
                NavMeshAgent.enabled = false;
            }
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            Vector3 velocity = NavMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxLengthNavPath) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float distance = 0;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return distance;
        }


        public void MoveTo(Vector3 destination)
        {
           NavMeshAgent.destination = destination;
           NavMeshAgent.isStopped = false;
        }
        public void StartMoveAction(Vector3 destination)
        {
            actionSheduler.StartAction(this);
            MoveTo(destination);
        }

        public void Cancel()
        {
            if(NavMeshAgent.isActiveAndEnabled)
            {
                NavMeshAgent.isStopped = true;
            }        
        }

        public object CaptureState()
        {
            var currentPosition = new SerializableVector3(transform.position);
            return currentPosition;
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3) state;
            NavMeshAgent.enabled = false;
            transform.position = position.ToVector();
            NavMeshAgent.enabled = true;
        }
    }
}