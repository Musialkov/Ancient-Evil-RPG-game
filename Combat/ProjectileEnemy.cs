using RPG.Attributes;
using UnityEngine.Events;
using UnityEngine;

namespace RPG.Combat
{
    public class ProjectileEnemy : Projectile
    {
        protected override void OnTriggerEnter(Collider other)
        {
            Health shootedObject = other.gameObject.GetComponent<Health>();

            onHit.Invoke();

            if (shootedObject == target && !target.IsDead())
            {
                shootedObject.TakeDamage(instigator, damage);
            }

            if (other.gameObject.tag != "Enemy")
            {
                Destroy(gameObject);
                SpawnHitImpact();
            }
        }
    }
}
