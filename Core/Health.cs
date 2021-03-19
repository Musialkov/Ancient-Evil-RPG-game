using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;

        Animator animator;
        ActionSheduler actionSheduler;

        private void Start()
        {
            animator = GetComponent<Animator>();
            actionSheduler = GetComponent<ActionSheduler>();
            CheckHealthPoint();
        }

        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            CheckHealthPoint();
        }

        private void CheckHealthPoint()
        {
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if(isDead) return;
            animator.SetTrigger("die");
            actionSheduler.CancelCurrentAction();
            isDead = true;
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float) state;    
        }
    }
}