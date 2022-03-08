using UnityEngine;
using RPG.Movement;
using RPG.Core;
using Musialkov.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine.Events;
using Musialkov.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] UnityEvent onHit;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float timeSinceLastAttack = 1f;
        [SerializeField] Transform rightHandTransform;
        [SerializeField] Transform leftHandTransform;
        [SerializeField] WeaponConfig defaultWeaponConfig;

        Health targetHealth = null;
        Mover mover;
        Animator animator;
        ActionSheduler actionSheduler;
        WeaponConfig currentWeaponConfig = null;
        Equipment equipment;
        float timeFromLastAttack = 0;
        float damageSpread = 0.15f;
        public Weapon currentWeapon;

        public float getTimeFromLastAttack()   { return timeFromLastAttack; }
        public void resetTimeFromLastAttack() { timeFromLastAttack = 0; }

        private void Awake() 
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            actionSheduler = GetComponent<ActionSheduler>();
            equipment = GetComponent<Equipment>();

            currentWeaponConfig = defaultWeaponConfig;
        }

        private void Start() 
        {        
            currentWeapon = EquipWeapon(currentWeaponConfig);                 
        }

        private void Update()
        {
            GettingToTarget();
        }

        public Weapon EquipWeapon(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            Weapon weapon = weaponConfig.SpawnWeapon(animator, rightHandTransform, leftHandTransform);
            currentWeapon = weapon;
            return weapon;
        }

        private void UpdateWeapon()
        {
           
        }

        public Health GetTargetHealth()
        {
            return targetHealth;
        }

        private void GettingToTarget()
        {
            timeSinceLastAttack += Time.deltaTime;
            timeFromLastAttack += Time.deltaTime;

            if (targetHealth == null) return;
            if (targetHealth.IsDead()) return;

       
            if (Vector3.Distance(transform.position, targetHealth.transform.position) > currentWeaponConfig.Range)   
            {
                mover.MoveTo(targetHealth.transform.position);
            }                         
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(targetHealth.transform.position);
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                //this will trigger Hit() event             
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");       
                timeSinceLastAttack = 0;
                timeFromLastAttack = 0;
            }            
        }
        

        //Animation event
        private void Hit()
        {
            if(targetHealth != null)
            {
                float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
                damage = CalculateDamageSpread(damage);

                if(currentWeapon != null)
                {           
                    currentWeapon.OnHit();
                }

                if(currentWeaponConfig.HasProjectile())
                {
                    currentWeaponConfig.SpawnProjectile(rightHandTransform, leftHandTransform, targetHealth, gameObject, damage);
                    Mana mana = GetComponent<Mana>();
                    if (mana != null && currentWeaponConfig.ManaCost > 0)
                    {                        
                        mana.ReduceMana(currentWeaponConfig.ManaCost);
                        if(mana.ManaValue < currentWeaponConfig.ManaCost)
                        {
                            Cancel();
                            currentWeaponConfig = defaultWeaponConfig;
                            currentWeapon = EquipWeapon(currentWeaponConfig);
                        }
                    }
                }
                else
                {
                    if(targetHealth.GetHealth() > 0)
                    {
                        targetHealth.TakeDamage(gameObject, damage);
                    }                    
                }                
            } 

            onHit.Invoke();       
        }

        //Animation event
        private void Shoot()
        {
            Hit();
        }

        public void Cancel()
        {
            targetHealth = null;
            StopAttackAnimation();
        }

        public void Attack (GameObject combatTarget)
        {
            actionSheduler.StartAction(this);
            if(targetHealth != combatTarget.GetComponent<Health>() && targetHealth != null)
            {
                StopAttackAnimation();
            }
            targetHealth = combatTarget.GetComponent<Health>();            
        }
        
        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) return false;
            if(!mover.CanMoveTo(combatTarget.transform.position)) return false;

            Mana mana = GetComponent<Mana>();
            if(mana != null && currentWeaponConfig.ManaCost > mana.ManaValue) return false;
            

            Health checkedTarget = combatTarget.GetComponent<Health>();
            return checkedTarget != null && !checkedTarget.IsDead();
        }

        private float CalculateDamageSpread(float damage)
        {
            float totalDamage;
            bool addDamage = Random.Range(0, 1) == 1;
            int spreadDmg = (int) Random.Range(0, damage * damageSpread);
            if(addDamage) totalDamage = damage + spreadDmg;
            else totalDamage = damage - spreadDmg;

            return totalDamage;
        }

        private void StopAttackAnimation()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
            timeSinceLastAttack = timeBetweenAttacks;
        }

        public object CaptureState()
        {
            if(currentWeaponConfig != null)
            {
                return currentWeaponConfig.name;
            }
            return null;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string) state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            currentWeapon = EquipWeapon(weapon);
        }

    }
}
