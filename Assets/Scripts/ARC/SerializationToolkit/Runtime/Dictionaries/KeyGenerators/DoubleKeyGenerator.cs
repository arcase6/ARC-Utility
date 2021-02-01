using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARC.SerializationToolkit.Dictionaries.KeyGenerators
{
    /// <summary>
    /// A generator that can be used to generate default keys when inserting doubleo a dictionary.
    /// </summary>
    public class DoubleKeyGenerator : IEnumerable<double>
    {
        public IEnumerator<double> GetEnumerator()
        {
            double index = 1;
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