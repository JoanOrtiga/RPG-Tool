using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGCreator.Editor.InventorySystem
{
    public class InventorySystemWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        private IS_Editor _inventorySystemEditor;
    
        [MenuItem("Tools/InventorySystem")]
        public static void ShowExample()
        {
            InventorySystemWindow wnd = GetWindow<InventorySystemWindow>();
            wnd.titleContent = new GUIContent("InventorySystem");
            wnd.minSize = new Vector2(500, 500);
        }
    
        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            VisualElement uxml = m_VisualTreeAsset.Instantiate();
            root.Add(uxml);
            _inventorySystemEditor.Bind(this, this);
        }

        private void OnEnable()
        {
            _inventorySystemEditor = new IS_Editor();
        }

        private void OnDisable()
        {
            _inventorySystemEditor.OnDisable();
        }

        private void OnGUI()
        {
            _inventorySystemEditor.OnGUI();
        }
    }
}


