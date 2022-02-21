using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using RPGCreator.InventorySystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGCreator.Editor.InventorySystem
{
    public class IS_TopBar : IVisualElementController
    {
        private const string SavedatabaseIndex = "SelectedDatabase";

        private IS_Editor _isEditor;
        
        public EditorWindow EditorWindow { get; set; }

        private DropdownField _selectDatabase;

        private bool _updateSelectDatabase;

        private List<string> _savedDatabasePaths = new List<string>();

        public ItemDatabase ItemDatabase { get; private set; }
        public Action<ItemDatabase> OnDatabaseUpdate;
        
        public void Bind(EditorWindow editorWindow, object sender)
        {
            _isEditor = sender as IS_Editor;
            EditorWindow = editorWindow;
            _selectDatabase = EditorWindow.rootVisualElement.Q("SelectDatabase") as DropdownField;
            _selectDatabase.RegisterValueChangedCallback(OnDatabaseChanged);

            UpdateSelectDatabase();
            _selectDatabase.index = PlayerPrefs.GetInt(SavedatabaseIndex, 0);
        }

        public void OnDisable()
        {
            
        }

        public void OnGUI()
        {
            if (EditorWindow.rootVisualElement.focusController.focusedElement ==
                _selectDatabase.focusController.focusedElement && _updateSelectDatabase)
            {
                _updateSelectDatabase = false;
                UpdateSelectDatabase();
            }
            
            if(!_updateSelectDatabase)
            {
                _updateSelectDatabase = true;
            }
        }

        private void UpdateSelectDatabase()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(ItemDatabase)}");

            _selectDatabase.choices.Clear();
            _savedDatabasePaths.Clear();

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                _savedDatabasePaths.Add(path);
                _selectDatabase.choices.Add(Path.GetFileNameWithoutExtension(path));
            }

            _selectDatabase.choices.Add("-> Create new database");
        }

        void OnDatabaseChanged(ChangeEvent<string> changeEvent)
        {
            int newIndex = _selectDatabase.choices.IndexOf(changeEvent.newValue);
            PlayerPrefs.SetInt(SavedatabaseIndex, newIndex);

            if (_selectDatabase.choices.Count-1 == newIndex)
            {
                string newSelection = NewDatabaseDialogue();
                if (newSelection != string.Empty)
                {
                    UpdateSelectDatabase();
                    _selectDatabase.index = _selectDatabase.choices.IndexOf(newSelection);
                }
                else
                {
                    _selectDatabase.index = _selectDatabase.choices.IndexOf(changeEvent.previousValue);
                }

                return;
            }

            ItemDatabase = AssetDatabase.LoadAssetAtPath<ItemDatabase>(_savedDatabasePaths[newIndex]);
            OnDatabaseUpdate.Invoke(ItemDatabase);
        }

        private string NewDatabaseDialogue()
        {
            string path = EditorUtility.SaveFilePanelInProject("Database", "Itemdatabase.asset", "asset",
                "Please enter a file name to save the database");
            
            if (path.Length != 0)
            {
                ItemDatabase itemDatabase = ScriptableObject.CreateInstance<ItemDatabase>();
                AssetDatabase.CreateAsset(itemDatabase, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                return Path.GetFileNameWithoutExtension(path);
            }

            return string.Empty;
        }
    }
}
