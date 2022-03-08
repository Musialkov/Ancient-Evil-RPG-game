using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 10f;
        [SerializeField] float timeToDisappear = 10f;
        [SerializeField] bool isTracking = false;
        [SerializeField] GameObject hitImpact;
        [SerializeField] protected UnityEvent onHit;

        protected float damage = 0;
        protected Health target;
        protected GameObject instigator = null;

        void Start()
        {
            transform.LookAt(GetAimLocation());
            Destroy(gameObject, timeToDisappear);
        }

        void Update()
        {
            if (target != null)
            {
                if (isTracking && !target.IsDead())
                {
                    transform.LookAt(GetAimLocation());
                }
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCollider.height / 2;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            Health shootedObject = other.gameObject.GetComponent<Health>();
            
            onHit.Invoke();
        
            if (shootedObject == target && !target.IsDead())
            {
                shootedObject.TakeDamage(instigator, damage);
            }

            Destroy(gameObject);
            SpawnHitImpact();            
        }

        protected void SpawnHitImpact()
        {
            if (hitImpact != null)
            {
                Instantiate(hitImpact, transform.position, Quaternion.identity);
            }
        }

    }
}
