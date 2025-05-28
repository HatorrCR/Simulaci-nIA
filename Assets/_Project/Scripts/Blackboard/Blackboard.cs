using UnityEngine;
using System.Collections.Generic;

namespace IAEngine.Shared
{
    public class Blackboard : MonoBehaviour
    {
        public static Blackboard Instance { get; private set; }
        private Dictionary<string, object> data = new();

        void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public void SetValue(string key, object value)
        {
            data[key] = value;
        }

        public T GetValue<T>(string key)
        {
            if (data.TryGetValue(key, out object value) && value is T typedValue)
                return typedValue;
            return default;
        }

        public bool HasKey(string key) => data.ContainsKey(key);
    }
}
