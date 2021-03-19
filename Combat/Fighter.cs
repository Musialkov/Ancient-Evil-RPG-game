using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float damage = 10f;
        [SerializeField] float range = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float timeSinceLastAttack = 1f;
        [SerializeField] GameObject weapon;
        [SerializeField] Transform handTransform;


        Health target;
        Mover mover;
        Animator animator;
        ActionSheduler actionSheduler;

        private void Start() 
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            actionSheduler = GetComponent<ActionSheduler>();
            SpawnWeapon();
        }

        private void SpawnWeapon()
        {
            if(weapon != null)
            {
                Instantiate(weapon, handTransform);
            }    
        }

        private void Update()
        {
            GettingToTarget();
        }

        private void GettingToTarget()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;
       
            if (Vector3.Distance(transform.position, target.transform.position) > range)   
            {
                mover.MoveTo(target.transform.position);
            }                         
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform.position);
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                //this will trigger Hit() event
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");       
                timeSinceLastAttack = 0;
            }            
        }

        //Animation event
        private void Hit()
        {
            if(target != null)
            {
                target.TakeDamage(damage);
                print("jebudu");
            }        
        }

        public void Cancel()
        {
            target = null;
            StopAttackAnimation();
        }

        public void Attack (GameObject combatTarget)
        {
            actionSheduler.StartAction(this);
            if(target != combatTarget.GetComponent<Health>() && target != null)
            {
                StopAttackAnimation();
            }
            target = combatTarget.GetComponent<Health>();
        }
        
        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) return false;
            Health checkedTarget = combatTarget.GetComponent<Health>();
            return checkedTarget != null && !checkedTarget.IsDead();
        }

        private void StopAttackAnimation()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
            timeSinceLastAttack = timeBetweenAttacks;
        }

    }
}
