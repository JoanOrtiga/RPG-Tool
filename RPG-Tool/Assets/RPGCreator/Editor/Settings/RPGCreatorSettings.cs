using RPGCreator.InventorySystem;
using UnityEngine;

namespace RPGCreator.Editor
{
    [CreateAssetMenu(fileName = "RPGCreatorSettings", menuName = "RPGCreator/Internal/Settings")]
    public sealed class RPGCreatorSettings : ScriptableObject
    {
        [SerializeField] private InventorySystemSettings inventorySystemSettings;
        public InventorySystemSettings GetInventorySystemSettings => inventorySystemSettings;
        [SerializeField] private VisualSettings visualSettings;
        public VisualSettings GetVisualSettings => visualSettings;

    }

    [System.Serializable]
    public sealed class InventorySystemSettings
    {
        public ItemDatabase inventoryDataBase;
    }

    [System.Serializable]
    public sealed class VisualSettings
    {
        public bool randomSetting;
        public bool randomSetting2;
        public float thisIsRandomValue;
        public Vector2 blabla;
    }
}