using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPGCreator.Editor.InventorySystem
{
    public class IS_ContentEditor : IVisualElementController
    {
        public EditorWindow EditorWindow { get; set; }

        public void Bind(EditorWindow editorWindow, object sender)
        {
            EditorWindow = editorWindow;
        }

        public void OnDisable()
        {
            
        }

        public void OnGUI()
        {
            
        }
    }
}

