using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        NavMeshAgent navMeshAgent;
        Health health;
        ActionSheduler actionSheduler;
        Animator animator;

        private void Awake() 
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            actionSheduler = GetComponent<ActionSheduler>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if(health.IsDead())
            {
                navMeshAgent.enabled = false;
            }

            UpdateAnimation();
        }

        public void MoveTo(Vector3 destination)
        {
           navMeshAgent.destination  = destination;
           navMeshAgent.isStopped = false;
        }
        public void StartMoveAction(Vector3 destination)
        {
            actionSheduler.StartAction(this);
            MoveTo(destination);
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimation()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 loclaVelocity = transform.InverseTransformDirection(velocity);
            float speed = loclaVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3) state;
            transform.position = position.ToVector();
        }
    }
}