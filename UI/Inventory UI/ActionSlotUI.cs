﻿using System.Collections;
using System.Collections.Generic;
using RPG.UI.Dragging;
using Musialkov.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;

        ActionStore store;

        private void Awake()
        {
            store = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionStore>();
            store.storeUpdated += UpdateIcon;
        }

        public void AddItems(InventoryItem item, int number)
        {
            store.AddAction(item, index, number, false);
        }

        public InventoryItem GetItem()
        {
            return store.GetAction(index);
        }

        public int GetNumber()
        {
            return store.GetNumber(index);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return store.MaxAcceptable(item, index);
        }

        public void RemoveItems(int number)
        {
            store.RemoveItems(index, number);
        }

        void UpdateIcon()
        {
            icon.SetItem(GetItem(), GetNumber());
        }
    }
}
