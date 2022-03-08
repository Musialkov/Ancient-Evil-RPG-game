using System.Collections;
using System.Collections.Generic;
using Musialkov.Inventories;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestItemGiver : MonoBehaviour
    {
        [SerializeField] List<Reward> rewards = new List<Reward>();
        [SerializeField] InventoryItem itemToDestroy;

        [System.Serializable]
        public class Reward
        {
            [Min(1)]
            public int number;
            public InventoryItem item;
        }

        Inventory inventory;
        ItemDropper itemDropper;

        private void Awake() 
        {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            itemDropper = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemDropper>();
        }

       
        public void GiveItems()
        {
            foreach (var reward in rewards)
            {
                bool canAddItem = inventory.AddToFirstEmptySlot(reward.item, reward.number);
                if (!canAddItem)
                {
                    itemDropper.DropItem(reward.item, reward.number);
                }
            }
        }

        public void RemoveItem()
        {
            inventory.RemoveItem(itemToDestroy);
        }
    }

}