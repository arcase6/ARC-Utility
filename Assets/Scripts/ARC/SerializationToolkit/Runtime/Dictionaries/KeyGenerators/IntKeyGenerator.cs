using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARC.SerializationToolkit.Dictionaries.KeyGenerators
{
    /// <summary>
    /// A generator that can be used to generate default keys when inserting into a dictionary.
    /// </summary>
    public class IntKeyGenerator : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            int index = 1;
            while (true)
            {
                yield return index++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }
}