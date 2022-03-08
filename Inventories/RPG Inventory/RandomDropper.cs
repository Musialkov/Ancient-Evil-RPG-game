using System.Collections;
using System.Collections.Generic;
using Musialkov.Inventories;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [SerializeField] float dropDistance = 2f;
        [SerializeField] float minDropDistance = 1f;
        [SerializeField] DropLibrary dropLibrary;

        const int ATTEMPTS = 25;

        public void RandomDrop()
        {
            var baseStats = GetComponent<BaseStats>();

            var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());
            foreach (var drop in drops)
            {
                DropItem(drop.item, drop.number);
            }
        }

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * dropDistance;
                NavMeshHit hit;
                if(Vector3.Distance(transform.position, randomPoint) < minDropDistance) continue;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position;
        }
    }
}
