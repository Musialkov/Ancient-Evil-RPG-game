using System.Collections;
using System.Collections.Generic;
using Musialkov.Inventories;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Action Item/Potion"))]
    public class Potion : ActionItem
    {
        [SerializeField] int healthBoost;
        [SerializeField] int mamaBoost;

        Health health;
        Mana mana;

        public override void Use(GameObject user)
        {
            base.Use(user);
            health = user.GetComponent<Health>();
            mana = user.GetComponent<Mana>();

            health.BoostHealth(healthBoost);
            mana.BoostMana(mamaBoost);
        } 
    }
}
