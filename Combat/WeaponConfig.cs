using UnityEngine;
using RPG.Attributes;
using RPG.Stats;
using Musialkov.Inventories;
using System.Collections.Generic;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG/Inventory/Weapon", order = 0)]
    public class WeaponConfig : ActionItem, IModifierProvider
    {
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] float percentageBonus = 0f;
        [SerializeField] float range = 2f;
        [SerializeField] float manaCost = 0f;
        [SerializeField] AnimatorOverrideController animatorOverride;
        [SerializeField] Weapon equipedWeapon;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile;

        const string weaponName = "Weapon";

        public float WeaponDamage { get => weaponDamage; set => weaponDamage = value; }
        public float PercentageBonus { get => percentageBonus; set => percentageBonus = value; }
        public float Range { get => range; set => range = value; }
        public float ManaCost { get => manaCost; set => manaCost = value; }

        public Weapon SpawnWeapon(Animator animator, Transform rightHand, Transform leftHand)
        {
            DestroyOldWeapon(rightHand, leftHand);
            Weapon weapon = null;
            if(equipedWeapon != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);

                weapon = Instantiate(equipedWeapon, handTransform);
                weapon.gameObject.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }   
            return weapon;               
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform previousWeapon = rightHand.Find(weaponName);
            if(previousWeapon == null)
            {
                previousWeapon = leftHand.Find(weaponName);
            }
            if(previousWeapon == null) return;

            previousWeapon.name = "IWillBeDestroyed:(";
            Destroy(previousWeapon.gameObject);
        }

        public void SpawnProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float damage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, damage);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public override void Use(GameObject user)
        {
            base.Use(user);
            Fighter fighter = user.GetComponent<Fighter>();
            if(fighter != null)
            {
                fighter.EquipWeapon(this);
            }
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return weaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return percentageBonus;
            }
        }
    }
}