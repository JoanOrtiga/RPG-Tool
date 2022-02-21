using System;
using System.Collections.Generic;
using System.Linq;
using RPGCreator.InventorySystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace RPGCreator.Editor.InventorySystem
{
    public class IS_LeftBar : IVisualElementController
    {
        private IS_Editor _isEditor;

        private Button _addNewItem;
        private ListView _itemList;
        private ToolbarPopupSearchField _searchField;

        private string searchingText = string.Empty;
        
        public EditorWindow EditorWindow { get; set; }

        private SerializedObject itemDatabase;
        private SerializedProperty itemList;

        public void Bind(EditorWindow editorWindow, object sender)
        {
            _isEditor = sender as IS_Editor;
            EditorWindow = editorWindow;

            _isEditor.OnDatabaseUpdate += OnDatabaseUpdate;

            _addNewItem = EditorWindow.rootVisualElement.Q("AddNewItem") as Button;
            _addNewItem.clicked += CreateNewItem;

            _searchField = EditorWindow.rootVisualElement.Q("SearchBar") as ToolbarPopupSearchField;
            _searchField.RegisterValueChangedCallback(OnSearching);
            
            SetupListView();
        }

        private void OnSearching(ChangeEvent<string> changeEvent)
        {
            searchingText = changeEvent.newValue;
            _itemList.RefreshItems();
        }

        private void SetupListView()
        {
            _itemList = EditorWindow.rootVisualElement.Q("ItemList") as ListView;

            Action<VisualElement, int> BindItem = (e, i) =>
            {
                if (!((Item)itemList.GetArrayElementAtIndex(i).objectReferenceValue).DisplayName.Contains(searchingText))
                {
                    e.parent.parent.visible = false;
                    EditorWindow.Repaint();
                    return;
                }

                e.parent.parent.visible = true;
                
                itemDatabase.Update();
                ((Label)e).text = (itemList.GetArrayElementAtIndex(i).objectReferenceValue as Item).DisplayName;
                ((Label)e).AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) =>
                {
                    itemDatabase.Update();
                    evt.menu.AppendAction("Clone", (e) =>
                    {
                        itemList.arraySize++;
                        itemList.InsertArrayElementAtIndex(itemList.arraySize);
                        var cloned =
                            ScriptableObject.Instantiate(
                                itemList.GetArrayElementAtIndex(i).objectReferenceValue as Item);
                        itemList.GetArrayElementAtIndex(itemList.arraySize - 1).objectReferenceValue = cloned;
                        itemDatabase.ApplyModifiedProperties();
                    });
                    evt.menu.AppendAction("Delete", (e) =>
                    {
                        RemoveItem(itemList.GetArrayElementAtIndex(i).objectReferenceValue);
                        itemList.DeleteArrayElementAtIndex(i);
                        itemDatabase.ApplyModifiedProperties();
                    });
                }));
            };
            _itemList.makeItem = () => { return new Label(); };
            _itemList.bindItem = BindItem;
            _itemList.showBoundCollectionSize = false;
        }

        public void OnDatabaseUpdate(ItemDatabase database)
        {
            //Update Database
            itemDatabase = new SerializedObject(database);
            itemList = itemDatabase.FindProperty("items");

            //Update listview
            _itemList.bindingPath = itemDatabase.FindProperty("items").name;
            _itemList.Bind(itemDatabase);
            _itemList.Rebuild();
        }

        public void OnDisable()
        {
            _searchField.UnregisterValueChangedCallback(OnSearching);
            _addNewItem.clicked -= CreateNewItem;
            _isEditor.OnDatabaseUpdate -= OnDatabaseUpdate;
        }

        public void OnGUI()
        {
        }

        private void RemoveItem(UnityEngine.Object objectToRemove)
        {
            AssetDatabase.RemoveObjectFromAsset(objectToRemove);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void CreateNewItem()
        {
            var item = ScriptableObject.CreateInstance<Item>();
            item.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(item, itemDatabase.targetObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            itemList.arraySize++;
            itemList.GetArrayElementAtIndex(itemList.arraySize - 1).objectReferenceValue = item;
            itemDatabase.ApplyModifiedProperties();
        }
    }
}