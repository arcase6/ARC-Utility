using System.Collections.Generic;
using ARC.SerializationToolkit.Dictionaries.Internal;
using SerializationToolkit;
using UnityEngine;

namespace ARC.SerializationToolkit.Runtime.Dictionaries
{

    [System.Serializable]
    public class UDictionary<TKey, TValue, TKeyValueGenerator> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver where TKeyValueGenerator : IEnumerable<TKey>, new()
    {
        //Used for serialization
        [SerializeField]
        protected List<UKeyValuePair<TKey, TValue>> items = new List<UKeyValuePair<TKey, TValue>>();

        /// <summary>
        /// Called right after object is deserialized. Need to move List of serializedData into dictionary
        /// </summary>
        public void OnAfterDeserialize()
        {
            this.Clear();
            var generator = new TKeyValueGenerator().GetEnumerator();
            foreach (UKeyValuePair<TKey, TValue> keyValuePair in items)
            {
                this.AddKeyOrChangeToDefault(keyValuePair, ref generator);
            }
        }

        private void AddKeyOrChangeToDefault(UKeyValuePair<TKey, TValue> keyValuePair, ref IEnumerator<TKey> generator)
        {
            while (this.ContainsKey(keyValuePair.Key) && generator.MoveNext())
            {
                keyValuePair.Key = generator.Current;
            }
            if (this.ContainsKey(keyValuePair.Key) == false)
            {
                this.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        /// <summary>
        /// Called right before object is serialized. Need to move dictionary values into the List
        /// </summary>
        public void OnBeforeSerialize()
        {
            items.Clear();
            foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
            {
                items.Add(new UKeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value));
            }
        }
    }

    public interface IDefaultKeyGenerator<TKey>
    {
        IEnumerable<TKey> GetDefaultKeyGenerator();
    }

    [System.Serializable]
    public class UDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver where TKey : IDefaultKeyGenerator<TKey>, new()
    {
        //Used for serialization
        [SerializeField]
        protected List<UKeyValuePair<TKey, TValue>> items = new List<UKeyValuePair<TKey, TValue>>();

        /// <summary>
        /// Called right after object is deserialized. Need to move List of serializedData into dictionary
        /// </summary>
        public void OnAfterDeserialize()
        {
            this.Clear();
            var generator = new TKey().GetDefaultKeyGenerator().GetEnumerator();
            foreach (UKeyValuePair<TKey, TValue> keyValuePair in items)
            {
                this.AddKeyOrChangeToDefault(keyValuePair, ref generator);
            }
        }

        private void AddKeyOrChangeToDefault(UKeyValuePair<TKey, TValue> keyValuePair, ref IEnumerator<TKey> generator)
        {
            while (this.ContainsKey(keyValuePair.Key) && generator.MoveNext())
            {
                keyValuePair.Key = generator.Current;
            }
            if (this.ContainsKey(keyValuePair.Key) == false)
            {
                this.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        /// <summary>
        /// Called right before object is serialized. Need to move dictionary values into the List
        /// </summary>
        public void OnBeforeSerialize()
        {
            items.Clear();
            foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
            {
                items.Add(new UKeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value));
            }
        }
    }

}

