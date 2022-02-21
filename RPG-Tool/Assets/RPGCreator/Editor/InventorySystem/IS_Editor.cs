using System;
using System.Collections.Generic;
using RPGCreator.InventorySystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGCreator.Editor.InventorySystem
{
    public class IS_Editor : IVisualElementController
    {
        private ItemDatabase _itemDatabase;
        
        private ListView _itemList;
        
        private IS_ContentEditor _contentEditor = new IS_ContentEditor();
        private IS_LeftBar _leftBar = new IS_LeftBar();
        private IS_TopBar _topBar = new IS_TopBar();

        public ItemDatabase ItemDatabase => _topBar.ItemDatabase;

        public Action<ItemDatabase> OnDatabaseUpdate
        {
            get => _topBar.OnDatabaseUpdate;
            set => _topBar.OnDatabaseUpdate = value;
        }
        public EditorWindow EditorWindow { get; set; }

        public void Bind(EditorWindow editorWindow, object sender)
        {
            EditorWindow = editorWindow;
            
            _leftBar.Bind(editorWindow, this);
            _topBar.Bind(editorWindow, this);
            _contentEditor.Bind(editorWindow, this);
        }

        public void OnDisable(){
            _leftBar.OnDisable();
            _topBar.OnDisable();
            _contentEditor.OnDisable();
        }

        public void OnGUI()
        {
            _topBar.OnGUI();
            _leftBar.OnGUI();
            _contentEditor.OnGUI();
        }
    }
}