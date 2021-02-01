using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARC.SerializationToolkit.Dictionaries.KeyGenerators
{
    /// <summary>
    /// A generator that can be used to generate default keys when inserting doubleo a dictionary.
    /// </summary>
    public class EnumKeyGenerator<TEnum> : IEnumerable<TEnum> where TEnum: struct, System.IConvertible
    {
        public IEnumerator<TEnum> GetEnumerator()
        {
            int index = 0;
            while (true)
            {
                yield return (TEnum)(object)(index++);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }
}