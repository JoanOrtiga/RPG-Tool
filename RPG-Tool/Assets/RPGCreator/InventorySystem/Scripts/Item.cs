using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCreator.InventorySystem
{
    [Serializable]
    public class Item : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private bool useIdAsDisplayName = true;
        [SerializeField] private string displayName;
        public string DisplayName => useIdAsDisplayName ? id : displayName;
        [SerializeField, TextArea] private string description;
        [SerializeField] private Sprite icon;
    }
}
