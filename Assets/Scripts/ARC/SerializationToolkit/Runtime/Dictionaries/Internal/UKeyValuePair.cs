namespace ARC.SerializationToolkit.Dictionaries.Internal
{
    [System.Serializable]
    public class UKeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public UKeyValuePair(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}

