using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SerializationToolkit
{
    public class ListSettingsAttribute : Attribute
    {
        public readonly float LabelWidth;
        public readonly string ItemPrefix;
        
        public ListSettingsAttribute(string itemPrefix = "Item", float labelWidth = -1)
        {
            LabelWidth = labelWidth;
            ItemPrefix = itemPrefix; 
        }
    }
    
    
    [Serializable]
    public class MixedList<TBase, T1, T2> : ISerializationCallbackReceiver, IList<TBase>, ICollection<TBase>,
        IEnumerable<TBase>, IEnumerable, IReadOnlyList<TBase>, IReadOnlyCollection<TBase>
        where T1 : TBase, new()
        where T2 : TBase, new()
    {
        public List<TBase> list { get; set; } = new List<TBase>();


        public static Type GetMemberType(int index)
        {
            switch (index)
            {
                case 0:
                    return typeof(T1);
                case 1:
                    return typeof(T2);
                default:
                    throw new IndexOutOfRangeException();
            }
        }


        #region Serialization

        [SerializeField] private List<STuple<int, T1>> m_Group1 = new List<STuple<int, T1>>();
        [SerializeField] private List<STuple<int, T2>> m_Group2 = new List<STuple<int, T2>>();

        public void OnBeforeSerialize()
        {
            /*m_Group1.Clear();
            m_Group2.Clear();
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var member = list[i];
                switch (member)
                {
                    case T1 t1:
                        m_Group1.Add(new STuple<int, T1>(i, t1));
                        break;
                    case T2 t2:
                        m_Group2.Add(new STuple<int, T2>(i, t2));
                        break;
                    default:
                        Debug.LogError("Invalid member in list. Unable to serialize. Removing :" + member?.ToString() );
                        list.RemoveAt(i);
                        break;
                }
            }*/
        }

        public void OnAfterDeserialize()
        {
            var array = new TBase[m_Group1.Count + m_Group2.Count];
            try
            {
                foreach (var (index, item) in m_Group1)
                    array[index] = item;
                foreach (var (index, item) in m_Group2)
                    array[index] = item;
                list = new List<TBase>(array);
            }
            catch
            {
                //problem happened deserializing. Likely due to unity writing unfinished serialized value
            }
        }

        #endregion

        #region List Wrapper

        public IEnumerator<TBase> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

        public void Add(TBase item)
        {
            if (ReferenceEquals(null, item)) throw new NullReferenceException();

            switch (item)
            {
                case T1 t1:
                    m_Group1.Add(new STuple<int, T1>(list.Count(), t1));
                    break;
                case T2 t2:
                    m_Group2.Add(new STuple<int, T2>(list.Count(), t2));
                    break;
                default:
                    Debug.LogError("Unserializable instance received :" + item?.GetType()?.ToString());
                    break;
            }

            list.Add(item);
        }


        public void Clear()
        {
            list.Clear();
            m_Group1.Clear();
            m_Group2.Clear();
        }

        public void RemoveAt(int index)
        {
            var foundIndex = 0;
            foundIndex = m_Group1.FindIndex(t => t.Item1 == index);
            if (foundIndex != -1)
            {
                m_Group1.RemoveAt(foundIndex);
            }
            else
            {
                foundIndex = m_Group1.FindIndex(t => t.Item1 == index);
                if (foundIndex == -1)
                    return;
                m_Group1.RemoveAt(foundIndex);
            }
            list.RemoveAt(index);
        }

        void ICollection<TBase>.Clear() => this.Clear();

        public bool Contains(TBase item) => list.Contains(item);

        public void CopyTo(TBase[] array, int arrayIndex)
        {
            this.Clear();
            int i = 0;
            foreach (var entry in array)
            {
                if (ReferenceEquals(null, entry))
                {
                    this.Clear();
                    throw new NullReferenceException();
                }

                switch (entry)
                {
                    case T1 t1:
                        m_Group1.Add(new STuple<int, T1>(i++, t1));
                        break;
                    case T2 t2:
                        m_Group2.Add(new STuple<int, T2>(i++, t2));
                        break;
                    default:
                        Debug.LogError("Unserializable instance received :" + entry ?.GetType()?.ToString());
                        this.Clear();
                        return;
                }
            }
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(TBase item)
        {
            var index = list.IndexOf(item);
            if(index != -1)
                this.RemoveAt(index);
            return index != -1;
        }


        public int Count => list.Count;
        
        int ICollection<TBase>.Count => list.Count;

        bool ICollection<TBase>.IsReadOnly => ((ICollection<TBase>) list).IsReadOnly;

        public int IndexOf(TBase item) => list.IndexOf(item);

        public void Insert(int index, TBase item)
        {
            //check for invalid input
            if (ReferenceEquals(null, item)) throw new NullReferenceException();
            if (item is T1 == false && item is T2 == false)
            {
                Debug.LogError("Unserializable instance received :" + item?.GetType()?.ToString());
                throw new ArgumentException();
            }
            //insert into list
            list.Insert(index, item);

            //update affected indexes
            foreach (var member in m_Group1)
                if (member.Item1 >= index)
                    member.Item1++;
            
            foreach (var member in m_Group2)
                if (member.Item1 >= index)
                    member.Item1++;
            
            //add newly added index at end of appropriate group
            switch (item)
            {
                case T1 t1:
                    m_Group1.Add(new STuple<int, T1>(index, t1));
                    break;
                case T2 t2:
                    m_Group2.Add(new STuple<int, T2>(index, t2));
                    break;
            }

        } 

        void IList<TBase>.RemoveAt(int index) => this.RemoveAt(index);

        public TBase this[int index]
        {
            get => list[index];
            set => SetValue(index, value);
        }

        private void SetValue(int index, TBase value)
        {
            //check validity of value
            if (ReferenceEquals(null, value)) throw new NullReferenceException();
            if (value is T1 == false && value is T2 == false)
            {
                Debug.LogError("Unserializable instance received :" + value ?.GetType()?.ToString());
                throw new ArgumentException();
            }
            
            //update list value
            list[index] = value;
            
            //update serialized data
            if (value is T1 t1)
            {
                foreach (var member in m_Group1)
                {
                    if (member.Item1 == index)
                    {
                        member.Item2 = t1;
                        return;
                    }
                }
            }

            if (value is T2 t2)
            {
                foreach (var member in m_Group2)
                {
                    if (member.Item1 == index)
                    {
                        member.Item2 = t2;
                        return;
                    }
                }
            }
        }

        int IReadOnlyCollection<TBase>.Count => ((IReadOnlyCollection<TBase>) list).Count;

        #endregion
    }
}