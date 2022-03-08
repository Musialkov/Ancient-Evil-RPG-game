using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using RPG.StoryLine;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class BossHealth : Health
    {
        bool canBeDestroyed = false;
        [SerializeField] List<LightFire> torches = new List<LightFire>();   

        public override void TakeDamage(GameObject instigator, float damage)
        {
            if(!canBeDestroyed) ChceckIfCanBeDestroyed();

            if(canBeDestroyed)
            {
                health = Mathf.Max(health - damage, 0);
                Debug.Log("Jestem tu");
                if (health > 0)
                {
                    onDamage.Invoke(damage);
                }
                CheckHealthPoint(instigator);
            }           
        }

        private void ChceckIfCanBeDestroyed()
        {
            var notRunningTorches = torches.Where(fire => !fire.IsActive).ToList();

            if(notRunningTorches.Count == 0) canBeDestroyed = true;
        }
    }
}