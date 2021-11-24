using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace RPGTool.Editors
{
    public class Settings : EditorWindow
    {
        [MenuItem("Tools/Settings")]
        public static void ShowExample()
        {
            Settings wnd = GetWindow<Settings>();
            wnd.titleContent = new GUIContent("Settings");
        }

        public void CreateGUI()
        {   
            VisualElement root = rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Settings.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);
        }
    }
}