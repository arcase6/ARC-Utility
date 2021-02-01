using ARC.SerializationToolkit.Dictionaries.KeyGenerators;

namespace ARC.SerializationToolkit.Runtime.Dictionaries
{
    [System.Serializable]
    public class IntDictionary<TValue> : UDictionary<int,TValue,IntKeyGenerator>
    {
    }
}