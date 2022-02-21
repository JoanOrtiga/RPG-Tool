using UnityEditor;

namespace RPGCreator.Editor
{
    public interface IVisualElementController
    {
        public EditorWindow EditorWindow { get; set; }
        public void Bind(EditorWindow editorWindow, object sender);
        public void OnDisable();
        public void OnGUI();
    }
}