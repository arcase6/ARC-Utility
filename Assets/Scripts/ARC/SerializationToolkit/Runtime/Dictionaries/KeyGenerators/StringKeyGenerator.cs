using System.Collections;
using System.Collections.Generic;

namespace ARC.SerializationToolkit.Dictionaries.KeyGenerators
{
    /// <summary>
    /// A generator that can be used to generate default keys when inserting into a dictionary.
    /// </summary>
    public class StringKeyGenerator : IEnumerable<string>
    {
        public IEnumerator<string> GetEnumerator()
        {
            string keyBase = "Key";
            int index = 1;
            while (true)
            {
                yield return keyBase + index;
                index++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }
}
