using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGCreator.Editor
{
    public class Settings : EditorWindow
    {
        private VisualSettings _visualSettings;

        [MenuItem("Tools/Settings")]
        public static void ShowExample()
        {
            Settings wnd = GetWindow<Settings>();
            wnd.titleContent = new GUIContent("Settings");
            wnd.minSize = new Vector2(400, 500);
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            /* VisualElement uxml = visualTreeAsset.Instantiate();
             root.Add(uxml);*/

            StyleSheet styleSheet = Utilities.FindUniqueAssetByName<StyleSheet>("Settings", "uss") as StyleSheet;
            var visualTree = Utilities.FindUniqueAssetByName<VisualTreeAsset>("Settings","uxml") as VisualTreeAsset;
            visualTree.CloneTree(root);
            
            Label label = new Label()
            {
                text = "VisualSettings"
            };

            label.AddToClassList("listIndexElement");
            label.styleSheets.Add(styleSheet);
            root.Q("ListIndex").Add(label);
        }
    }
}