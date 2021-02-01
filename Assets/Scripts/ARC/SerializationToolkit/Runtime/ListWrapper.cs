using System;
using System.Collections;
using System.Collections.Generic;

namespace SerializationToolkit
{
    [Serializable]
    public class ListWrapper<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>
    {
        public List<T> list = new List<T>();

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) list).GetEnumerator();

        public void Add(T item) => list.Add(item);

        int IList.Add(object value) => ((IList) list).Add(value);

        void IList.Clear() => list.Clear();

        bool IList.Contains(object value) => ((IList) list).Contains(value);

        int IList.IndexOf(object value) => ((IList) list).IndexOf(value);

        void IList.Insert(int index, object value) => ((IList) list).Insert(index, value);

        void IList.Remove(object value) => ((IList) list).Remove(value);

        void IList.RemoveAt(int index) => ((IList) list).RemoveAt(index);

        bool IList.IsFixedSize => ((IList) list).IsFixedSize;

        bool IList.IsReadOnly => ((IList) list).IsReadOnly;

        object IList.this[int index]
        {
            get => ((IList) list)[index];
            set => ((IList) list)[index] = value;
        }



        public void Clear() => list.Clear();

        public bool Contains(T item) => list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        public bool Remove(T item) => list.Remove(item);

        void ICollection.CopyTo(Array array, int index) => ((ICollection) list).CopyTo(array, index);

        public int Count => list.Count;

        public bool IsSynchronized => ((ICollection) list).IsSynchronized;
        public object SyncRoot => ((ICollection) list).SyncRoot;

        int ICollection<T>.Count => list.Count;

        bool ICollection<T>.IsReadOnly => ((ICollection<T>) list).IsReadOnly;

        public int IndexOf(T item) => list.IndexOf(item);

        public void Insert(int index, T item) => list.Insert(index, item);

        public void RemoveAt(int index) => list.RemoveAt(index);

        public T this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        int IReadOnlyCollection<T>.Count { get; }
    }
}