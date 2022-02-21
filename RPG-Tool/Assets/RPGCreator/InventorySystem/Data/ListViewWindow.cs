using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ListViewWindow : EditorWindow
{
    [MenuItem("Window/UI Toolkit/ListViewBindingExample")]
    public static void ShowExample()
    {
        ListViewWindow wnd = GetWindow<ListViewWindow>();
        wnd.titleContent = new GUIContent("ListViewBindingExample");
    }

    [SerializeField] private List<int> m_Numbers;

    private ListView m_ListView;
    private SerializedObject serializedObject;
    
    
    public enum DisplayArrayType
    {
        IntegerNumbers,
        CustomStructs
    }

    [SerializeField]
    private DisplayArrayType m_DisplayArrayType;

    private void CreateGUI()
    {
        serializedObject = new SerializedObject(this);
        VisualElement root = rootVisualElement;
        
        var rowContainer = new VisualElement();
        rowContainer.style.flexDirection = FlexDirection.Row;
        rowContainer.style.justifyContent = Justify.FlexStart;
 
        var dataSelector = new EnumField();
        dataSelector.bindingPath = nameof(m_DisplayArrayType);
        dataSelector.RegisterValueChangedCallback(SwitchDisplayedData);

        root.Add(dataSelector);
        
        root.Bind(serializedObject);
        
        
        m_ListView = new ListView();
        m_ListView.showBoundCollectionSize = false;
        m_ListView.itemHeight = 20;
        m_ListView.reorderable = true;
        m_ListView.reorderMode = ListViewReorderMode.Animated;
        rootVisualElement.Insert(1, m_ListView);
        
        m_ListView.bindingPath = nameof(m_Numbers);
        m_ListView.Bind(serializedObject);

        m_Numbers = new List<int>();
        m_Numbers.Add(1);
        m_Numbers.Add(3);
        m_Numbers.Add(5);
        
        

       /*
 
        root.Add(dataSelector);
 
        AddButton(rowContainer, "Default", () => CreateListView(false, false));
        AddButton(rowContainer, "Custom MakeItem", () => CreateListView(true, false));
        AddButton(rowContainer, "Custom MakeItem+BindItem", () => CreateListView(true, true));
 
        root.Add(rowContainer);
       
        CreateListView(false, false);
       
        rowContainer = new VisualElement();
        rowContainer.style.flexDirection = FlexDirection.Row;
        rowContainer.style.justifyContent = Justify.FlexEnd;
        root.Add(rowContainer);
 
       
        AddButton(rowContainer, "-", DecreaseArraySize);
        AddButton(rowContainer, "+", IncreaseArraySize);
       */
    }
    
    private void SwitchDisplayedData(ChangeEvent<Enum> evt)
    {
        var newValue = (DisplayArrayType) evt.newValue;
 
        if (m_DisplayArrayType != newValue)
 
        {
            // Because we're hooked before the bindings system, we'll receive value changes before it.
            // So we need to affect the value right away before recreating our list
            m_DisplayArrayType = newValue;
           // CreateListView(false, false);
        }
    }
    /*
    [Serializable]
    public struct CustomStruct
    {
        public string StringValue;
        public float FloatValue;
        public CustomStruct(string strValue, float fValue)
        {
            StringValue = strValue;
            FloatValue = fValue;
        }
    }
   
    
 
    [SerializeField] private List<int> m_Numbers;
    [SerializeField] private List<CustomStruct> m_CustomStructs;
 
   
    private ListView m_ListView;
    private SerializedObject serializedObject;
   
    private SerializedProperty m_ArraySizeProperty;
    private SerializedProperty m_ArrayProperty;
 
    private int m_ListViewInsertIndex = -1; // To make sure we insert the ListView at the right place in our visualTree
   
    public void CreateGUI()
    {
        CreateDataIfNecessary();
 
        serializedObject = new SerializedObject(this);
        VisualElement root = rootVisualElement;
 
        var rowContainer = new VisualElement();
        rowContainer.style.flexDirection = FlexDirection.Row;
        rowContainer.style.justifyContent = Justify.FlexStart;
 
        var dataSelector = new EnumField();
        dataSelector.bindingPath = nameof(m_DisplayArrayType);
       
        dataSelector.RegisterValueChangedCallback(SwitchDisplayedData);
 
        root.Add(dataSelector);
 
        AddButton(rowContainer, "Default", () => CreateListView(false, false));
        AddButton(rowContainer, "Custom MakeItem", () => CreateListView(true, false));
        AddButton(rowContainer, "Custom MakeItem+BindItem", () => CreateListView(true, true));
 
        root.Add(rowContainer);
       
        CreateListView(false, false);
       
        rowContainer = new VisualElement();
        rowContainer.style.flexDirection = FlexDirection.Row;
        rowContainer.style.justifyContent = Justify.FlexEnd;
        root.Add(rowContainer);
 
       
        AddButton(rowContainer, "-", DecreaseArraySize);
        AddButton(rowContainer, "+", IncreaseArraySize);
       
        root.Bind(serializedObject);
    }
 
    private void CreateDataIfNecessary()
    {
        if (m_Numbers == null)
        {
            m_Numbers = new List<int>() {1, 2, 3};
        }
 
        if (m_CustomStructs == null)
        {
            m_CustomStructs = new List<CustomStruct>();
            for (var i = 0; i < 3; ++i)
            {
                m_CustomStructs.Add(new CustomStruct($"Value number {i}", i + 0.5f));
            }
        }
    }
 
    private void IncreaseArraySize()
    {
        m_ArraySizeProperty.intValue++;
        serializedObject.ApplyModifiedProperties();
    }
 
    private void DecreaseArraySize()
    {
        if (m_ArraySizeProperty.intValue > 0)
        {
            m_ArraySizeProperty.intValue--;
            serializedObject.ApplyModifiedProperties();
        }
    }
 
 
    void AddButton(VisualElement container, string label, Action onClick)
    {
        container.Add(new Button(onClick) {text = label});
    }
 
    
 
    void CreateListView(bool customMakeItem, bool customBindItem)
    {
        if (m_ListView != null)
        {
            //We clean ourselves up
            m_ListView.Unbind();
            m_ListView?.RemoveFromHierarchy();
        }
       
        
 
        m_ListView.name = "List-" + m_DisplayArrayType.ToString();
       
        if (m_DisplayArrayType == DisplayArrayType.CustomStructs)
        {
            m_ListView.bindingPath = nameof(m_CustomStructs);
            m_ListView.itemHeight = 60;
        }
        else
        {
           
        }
 
        m_ArrayProperty = serializedObject.FindProperty(m_ListView.bindingPath);
        m_ArraySizeProperty = serializedObject.FindProperty(m_ListView.bindingPath + ".Array.size");
       
        m_ListView.style.flexGrow = 1;
 
        if (customMakeItem || customBindItem)
        {
            m_ListView.name = m_ListView.name + "-custom-item";
            // You can have only make item (default bindItem should work)
            if (m_DisplayArrayType == DisplayArrayType.CustomStructs)
            {
                m_ListView.makeItem = () => CreateCustomStructListItem(customBindItem);
            }
            else
            {
                m_ListView.makeItem = () => CreateNumberListItem(customBindItem);
            }
        }
 
        if (customBindItem)
        {
            m_ListView.name += "+bind";
 
            // The default bindItem will find the first IBindable type and bind the property to it
            // If you really have specific use cases you might want to set your own bindItem
         
 
            m_ListView.bindItem = ListViewBindItem;
        }
 
        if (m_ListViewInsertIndex < 0)
        {
            m_ListViewInsertIndex = rootVisualElement.childCount;
            rootVisualElement.Add(m_ListView);
               
        }
    }
 
    private void AddRemoveItemButton(VisualElement row, bool enable)
    {
        var button = new Button() {text = "-"};
        button.RegisterCallback<ClickEvent>((evt) =>
        {
            var clickedElement = evt.target as VisualElement;
 
            if (clickedElement != null && clickedElement.userData is int index)
            {
                m_ArrayProperty.DeleteArrayElementAtIndex(index);
                serializedObject.ApplyModifiedProperties();
            }
        });
 
        if (enable)
        {
            button.tooltip = "Remove this item from the list";
        }
        else
        {
            button.SetEnabled(false);
            row.tooltip = "Item removing is only available with custom BindItem";
        }
        row.Add(button);
    }
   
    VisualElement CreateCustomStructListItem(bool removeButtonAvailable)
    {
        var keyFrameContainer = new BindableElement(); //BindableElement so the default bind can assign the item's root property
        var lbl = new Label("Custom Item UI");
        lbl.AddToClassList("custom-label");
        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.justifyContent = Justify.SpaceBetween;
        row.Add(lbl);
 
        AddRemoveItemButton(row, removeButtonAvailable);
 
        keyFrameContainer.Add(row);
        keyFrameContainer.Add(new TextField() {bindingPath = nameof(CustomStruct.StringValue)});
        keyFrameContainer.Add(new FloatField() {bindingPath = nameof(CustomStruct.FloatValue)});
        return keyFrameContainer;
    }
   
    VisualElement CreateNumberListItem(bool removeButtonAvailable)
    {
        var row = new VisualElement(); //BindableElement so the default bind can assign the item's root property
        row.style.flexDirection = FlexDirection.Row;
        row.style.justifyContent = Justify.SpaceBetween;
 
        row.Add(new Label()); // default bind need this to be the first Bindable in the tree
        AddRemoveItemButton(row, removeButtonAvailable);
        return row;
    }
   
    void ListViewBindItem(VisualElement element, int index)
    {
        var label = element.Q<Label>(className: "custom-label");
        if (label != null)
        {
            label.text = "Custom Item UI (Custom Bound)";
        }
 
        var button = element.Q<Button>();
        if (button != null)
        {
            button.userData = index;
        }
 
        //we find the first Bindable
        var field = element as IBindable;
        if (field == null)
        {
            //we dig through children
            field = element.Query().Where(x => x is IBindable).First() as IBindable;
        }
 
        // Bound ListView.itemsSource is a IList of SerializedProperty
        var itemProp = m_ListView.itemsSource[index] as SerializedProperty;
 
        field.bindingPath = itemProp.propertyPath;
 
        element.Bind(itemProp.serializedObject);
    }
    */
    /*
    [MenuItem("Window/ListViewExampleWindow")]
    public static void OpenDemoManual()
    {
        GetWindow<ListViewWindow>().Show();
    }

    private ListView listView;
    private List<string> items;

    public void OnEnable()
    {
        // Create some list of data, here simply numbers in interval [1, 1000]
        const int itemCount = 1000;
        items = new List<string>(itemCount);
        for (int i = 1; i <= itemCount; i++)
            items.Add(i.ToString());

        // The "makeItem" function will be called as needed
        // when the ListView needs more items to render
        
        Label label = new Label();
        Func<VisualElement> makeItem = () => new Label();

        // As the user scrolls through the list, the ListView object
        // will recycle elements created by the "makeItem"
        // and invoke the "bindItem" callback to associate
        // the element with the matching data item (specified as an index in the list)
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            (e as Label).text = items[i];
            (e as Label).AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) =>
            {
                evt.menu.AppendAction("Delete", (e) =>
                {
                    Debug.Log(items[i]);
                    
                    UpdateList(i);
                });
            }));
        };

        // Provide the list view with an explict height for every row
        // so it can calculate how many items to actually display
        const int itemHeight = 16;

        listView = new ListView(items, itemHeight, makeItem, bindItem);

        listView.selectionType = SelectionType.Multiple;

        listView.onItemsChosen += obj => Debug.Log(obj);
        listView.onSelectionChange += objects => Debug.Log(objects);

        listView.style.flexGrow = 1.0f;

        rootVisualElement.Add(listView);
    }

    private void UpdateList(int i)
    {
        listView.itemsSource.Remove(items[i]);
        listView.RefreshItems();
    }*/
}
