using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCreator.InventorySystem
{
    [CreateAssetMenu(menuName = "RPGCreator/InventorySystem/ItemDatabase", fileName = "ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        [SerializeField] private List<Item> items;
        public void AddItem(Item item)
        {
            items.Add(item);
        }
    }
}
