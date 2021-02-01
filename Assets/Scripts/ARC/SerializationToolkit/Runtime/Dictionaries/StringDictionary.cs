using ARC.SerializationToolkit.Dictionaries;
using ARC.SerializationToolkit.Dictionaries.KeyGenerators;

namespace ARC.SerializationToolkit.Runtime.Dictionaries
{
    [System.Serializable]
    public class StringDictionary<TValue> : UDictionary<string,TValue,StringKeyGenerator>
    {
    }
}