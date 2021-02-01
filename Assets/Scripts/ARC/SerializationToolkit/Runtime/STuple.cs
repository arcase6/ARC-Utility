using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace SerializationToolkit
{
    [Serializable]
    public class STuple<T1, T2> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
    {
        [SerializeField] private T1 m_Item1;
        [SerializeField] private T2 m_Item2;

        public T1 Item1
        {
            get => m_Item1;
            set => m_Item1 = value;
        }

        public T2 Item2
        {
            get => m_Item2;
            set => m_Item2 = value;
        }

        public static Type MemberType(int index)
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

        public STuple(T1 item1, T2 item2)
        {
            m_Item1 = item1;
            m_Item2 = item2;
        }

        public void Deconstruct(out T1 item1, out T2 item2)
        {
            item1 = m_Item1;
            item2 = m_Item2;
        }

        public override bool Equals(object obj)
        {
            return ((IStructuralEquatable) this).Equals(obj, EqualityComparer<object>.Default);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            return other != null && other is STuple<T1, T2> tuple && comparer.Equals(m_Item1, tuple.m_Item1) && comparer.Equals(m_Item2, tuple.m_Item2);
        }

        public override int GetHashCode()
        {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            return CombineHashCodes(comparer.GetHashCode(m_Item1), comparer.GetHashCode(m_Item2));
        }

        private static int CombineHashCodes(int h1, int h2) => (h1 << 5) + h1 ^ h2;

        public int CompareTo(object other, IComparer comparer)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        object ITuple.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.Item1;
                    case 1:
                        return this.Item2;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        int ITuple.Length => 2;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            sb.Append(m_Item1);
            sb.Append(", ");
            sb.Append(m_Item2);
            sb.Append(")");
            return sb.ToString();
        }
    }
}