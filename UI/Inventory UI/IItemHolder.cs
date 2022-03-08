using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Musialkov.Inventories;

namespace RPG.UI.Inventories
{
    public interface IItemHolder
    {
        InventoryItem GetItem();
    }
}