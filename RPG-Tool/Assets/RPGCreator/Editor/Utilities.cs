using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGCreator.Editor
{
    public static class Utilities
    {
        public static void Log(string text)
        {
            Debug.Log($"RPGCreator: {text}");
        }

        public static void LogError(string text)
        {
            Debug.LogError($"RPGCreator: {text}");
        }
        
        public static object FindUniqueAsset<T>(string name, string extension = "asset")
        {
            string path = PlayerPrefs.GetString(name, string.Empty);
            if (path == string.Empty)
                path = FindPath<T>(name, extension);

            var asset = AssetDatabase.LoadAssetAtPath(path, typeof(T));

            if (asset == null)
            {
                if (path == string.Empty)
                    path = FindPath<T>(name, extension);

                asset = AssetDatabase.LoadAssetAtPath(path, typeof(T));

                if (asset == null)
                {
                    LogError($"{name}.{extension} not found.");
                    return null; 
                }
            }

            PlayerPrefs.SetString(name, path);
            return asset;
        }

        private static string FindPath<T>(string name, string extension)
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T));
            if (guids.Length == 0)
            {
                LogError($"No {name}.{extension} found. Make sure original '{name}.{extension}' is in the project.");
                return null;
            }

            if (guids.Length > 1)
            {
                LogError($"More than one {name}.{extension} found. That may cause problems. Make sure only original '{name}.{extension}' is in the project.");
            }

            return AssetDatabase.GUIDToAssetPath(guids[0]);
        }

        public static object FindUniqueAssetByName<T>(string name, string extension = "asset")
        {
            var enumerable = AssetDatabase.FindAssets($"{name} t:{typeof(T)}").Where((guid) => Path.GetFileName(AssetDatabase.GUIDToAssetPath(guid)) == $"{name}.{extension}");
            string[] guids = enumerable.ToArray();
            
            if (guids.Length == 0)
            {
                LogError($"No {name}.{extension} found. Make sure original '{name}.{extension}' is in the project.");
                return null;
            }

            if (guids.Length > 1)
            {
                LogError($"More than one {name}.{extension} found. That may cause problems. Make sure only original '{name}.{extension}' is in the project.");
            }

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var asset = AssetDatabase.LoadAssetAtPath(path, typeof(T));

            if (asset == null)
            {
                LogError($"{name}.{extension} not found.");
                return null;
            }
                
            return asset;
        }
    }
}