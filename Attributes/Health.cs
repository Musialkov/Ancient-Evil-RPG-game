using UnityEngine;
using Musialkov.Saving;
using RPG.Core;
using RPG.Stats;
using UnityEngine.Events;
using RPG.UI.Stats;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] protected TakeDamageEvent onDamage;
        [SerializeField] protected UnityEvent onDie;

        protected Animator animator;
        protected ActionSheduler actionSheduler;
        protected BaseStats baseStats;

        protected float health = -1f;
        protected bool isDead = false;

        ExperiencePopup expPopup;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            actionSheduler = GetComponent<ActionSheduler>();
            baseStats = GetComponent<BaseStats>();
            expPopup = GameObject.FindGameObjectWithTag("ExperiencePopup").GetComponent<ExperiencePopup>();           
        }

        private void Start() 
        {
            if (health < 0)
            {
                health = baseStats.GetStat(Stat.Health);
            }
            CheckHealthPoint(null);
        }

        private void OnEnable() 
        {
            baseStats.onLevelUp += SetMaximumHealth;
        }

        private void OnDisable() 
        {
            baseStats.onLevelUp -= SetMaximumHealth;
        }

        public float GetHealth()
        {
            return health;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public float GetPercentageHealth()
        {
            return GetFraction() * 100;
        }

        public float GetFraction()
        {
            return (health / baseStats.GetStat(Stat.Health));
        }

        public virtual void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if(health > 0)
            {
                onDamage.Invoke(damage);
            }       
            CheckHealthPoint(instigator);
        }

        public void BoostHealth(float healthBoost)
        {
            health += healthBoost;
            if (health > baseStats.GetStat(Stat.Health))
            {
                health = baseStats.GetStat(Stat.Health);
            }
        }

        protected void CheckHealthPoint(GameObject instigator)
        {
            if (health <= 0)
            {
                onDie.Invoke();
                Die();
                if (instigator == null) return;
                if (instigator.GetComponent<Experience>() == null) return;
                instigator.GetComponent<Experience>().GainExperience(baseStats.GetStat(Stat.GainedExperience));
            }
        }

        protected void SetMaximumHealth()
        {
            health = baseStats.GetStat(Stat.Health);
        }

        protected void Die()
        {
            if(isDead) return;
            if(gameObject.tag != "Player" && gameObject.tag != "Pickup")
            {
                StartCoroutine(expPopup.ShowPopup(baseStats.GetStat(Stat.GainedExperience)));
            }
            
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
            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}