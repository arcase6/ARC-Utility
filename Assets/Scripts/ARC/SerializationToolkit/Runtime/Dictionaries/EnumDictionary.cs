using ARC.SerializationToolkit.Dictionaries.KeyGenerators;

namespace ARC.SerializationToolkit.Runtime.Dictionaries
{
    [System.Serializable]
    public class EnumDictionary<TEnum,TValue> : UDictionary<TEnum,TValue,EnumKeyGenerator<TEnum>> where TEnum:struct, System.IConvertible
    {
    }
}