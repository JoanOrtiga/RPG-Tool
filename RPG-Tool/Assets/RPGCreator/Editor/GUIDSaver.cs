using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPGCreator.Editor
{
    [CreateAssetMenu(fileName = "GUIDSaver", menuName = "RPGCreator/Internal/GUIDSaver", order = 0)]
    public class GUIDSaver : ScriptableObject
    {
        [SerializeField,/* HideInInspector*/] private List<string> keys;
        [SerializeField,/* HideInInspector*/] private List<string> guids;
        
        public void AddGuid(string key, string guid)
        {
            if (keys.Contains(key))
            {
                Utilities.Log("There's already a GUID with that key.");
                return;
            }
            keys.Add(key);
            guids.Add(guid);
        }

        public bool KeyExists(string key)
        {
            return keys.Contains(key);
        }
        
        public string GetGuid(string key)
        {
            if (!keys.Contains(key))
            {
                Utilities.LogError("No GUID with that key");
            }
            return guids[keys.IndexOf(key)];
        }

        public void SetGuid(string key, string guid)
        {
            guids[keys.IndexOf(key)] = guid;
        }
    }
}