using UnityEditor;
using UnityEngine;

namespace RPGCreator.Editor
{
    [CreateAssetMenu(menuName = "RPGCreator/Internal/Editor References", fileName = "EditorReferences")]
    public class EditorReferences : ScriptableObject
    {
        [SerializeField, HideInInspector] private RPGCreatorSettings rpgCreatorSettings;

        private static EditorReferences _instance;
        public static EditorReferences Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Utilities.FindUniqueAsset<EditorReferences>("EditorReferences") as EditorReferences;
                    
                    if(_instance != null)
                    {
                        _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    }
                }

                return _instance;
            }
            set => _instance = value;
        }

        private void OnEnable()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            GetSettings();
        }

        public RPGCreatorSettings GetSettings()
        {
            if (rpgCreatorSettings == null)
            {
                rpgCreatorSettings = Utilities.FindUniqueAsset<RPGCreatorSettings>("RPGCreatorSettings") as RPGCreatorSettings;
            }

            return rpgCreatorSettings;
        }
    }
}
