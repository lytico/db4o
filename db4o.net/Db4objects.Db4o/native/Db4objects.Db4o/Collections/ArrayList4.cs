/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

#if !SILVERLIGHT

using System;
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Collections
{
    [Obsolete("Use Db4objects.Db4o.Collections.ActivatableList instead")]
    public partial class ArrayList4<E> : IList<E>, IList, IActivatable
    {
        #region Instance Variables

		[NonSerialized]
        private int modCount;

        #endregion

        public int IndexOf(E item)
        {
            return Array.IndexOf(GetElements(), item, 0, listSize);
        }

        public void Insert(int index, E item)
        {
            Add(index, item);
        }

        public int Add(object value)
        {
            CheckObjectType(value);
            Add((E) value);
            return Count - 1;
        }

        public bool Contains(object value)
        {
            CheckObjectType(value);
            return IndexOf(value) != -1;
        }

        public int IndexOf(object value)
        {
            CheckObjectType(value);
            IList<E> self = this;
            return self.IndexOf((E) value);
        }

        public void Insert(int index, object value)
        {
            CheckObjectType(value);
            Insert(index, (E) value);
        }

        public void Remove(object value)
        {
            CheckObjectType(value);
            Remove((E)value);
        }

        public void RemoveAt(int index)
        {
            RemoveImpl(index);
        }

        object IList.this[int index]
        {
            get { return Get(index); }
            set { Set(index, (E) value); }
        }

        public E this[int index]
        {
            get { return Get(index); }
            set { Set(index, value); }
        }

        public void Add(E item)
        {
            Add(Count, item);
        }

        public bool Contains(E item)
        {
            return Contains((object) item);
        }

        public void CopyTo(E[] array, int arrayIndex)
        {   
            CopyTo((Array) array, arrayIndex);
        }

        public bool Remove(E item)
        {
            int index = IndexOf(item);
            if (index == -1) return false;
            
            RemoveAt(index);
            return true;
        }

		public void CopyTo(Array array, int index)
		{
			if (null == array) throw new ArgumentNullException();
			if (array.Rank != 1) throw new ArgumentException();

			Array.Copy(GetElements(), 0, array, index, listSize);
		}

    	public object SyncRoot
        {
            get { throw new NotSupportedException(); }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public IEnumerator<E> GetEnumerator()
        {
			Activate(ActivationPurpose.Read);
            int version = modCount;
        	int size = listSize;
			for (int i = 0; i < size; ++i)
			{
				if (version != modCount)
				{
					throw new InvalidOperationException();
				}

				yield return elements[i];
			};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<E>) this).GetEnumerator();
        }

        public override string ToString()
        {
            return String.Format("ArrayList4<{0}> (Count={1})", typeof(E).Name, Count);
        }

        #region Facility methods

        public void AddRange(IEnumerable<E> collection)
        {
        }
    
        public int BinarySearch(E item)
        {
            return Array.BinarySearch(GetElements(), item);
        }

        public int BinarySearch(E item, IComparer<E> comparer)
        {
            return Array.BinarySearch(GetElements(), item, comparer);
        }

        public int BinarySearch(int index, int count, E item, IComparer<E> comparer)
        {
            return Array.BinarySearch(GetElements(), index, count, item, comparer);
        }

        #if !CF

        public ArrayList4<TOutput> ConvertAll<TOutput>(Converter<E, TOutput> converter)
        {
            return new ArrayList4<TOutput>(Array.ConvertAll(GetElements(), converter));
        }

        public bool Exists(Predicate<E> match)
        {
            return Array.Exists(GetElements(), match);
        }

        public E Find(Predicate<E> match)
        {
            return Array.Find(GetElements(), match);
        }

        public ArrayList4<E> FindAll(Predicate<E> match)
        {
            return new ArrayList4<E>(Array.FindAll(GetElements(), match));
        }
    
        public int FindIndex(int startIndex, int count, Predicate<E> match)
        {
            return Array.FindIndex(GetElements(), startIndex, count, match);
        }

        public E FindLast(Predicate<E> match)
        {
            return Array.FindLast(GetElements(), match);
        }

        public int FindLastIndex(int startIndex, int count, Predicate<E> match)
        {
            return Array.FindLastIndex(GetElements(), startIndex, count, match);
        }
        
        public void ForEach(Action<E> action)
        {
            Array.ForEach(GetElements(), action);
        }

        #endif

        public void InsertRange(int index, IEnumerable<E> collection)
        {
            AddAllImpl(index, new List<E>(collection).ToArray());
        }
    
        public void RemoveRange(int index, int count)
        {
            RemoveRangeImpl(index, count);
        }
    
        public void Sort(int index, int count, IComparer<E> comparer)
        {
            Array.Sort(GetElements(), index, count, comparer);
        }
    
        public E[] ToArray()
        {
            E[] items = GetElements();
            return items == null ? items : (E[]) items.Clone();
        }

        public bool TrueForAll(Predicate<E> match)
        {
            return Array.TrueForAll(GetElements(), match);
        }

        #endregion

        #region Sharpen Helper Methods

        private static E[] CollectionToArray(ICollection<E> coll)
        {
            return new List<E>(coll).ToArray();
        }

        internal static void CheckIndex(int index, int from, int to)
        {
            if (index < from || index > to)
            {
                throw new ArgumentOutOfRangeException(String.Format("Index {0} must be in the range[{1} - {2}]", index, from, to));
            }
        }

        private static E[] AllocateStorage(int size)
        {
            return new E[size];
        }

        private static E DefaultValue()
        {
             return default(E);
        }

        #endregion

        #region Helper Methods
        
        private static void CheckObjectType(object value)
        {
            if (!(value is E))
            {
                throw new ArgumentException();
            }
        }

        private E[] GetElements()
        {
			Activate(ActivationPurpose.Read);
            return elements;
        }

        #endregion
    }
}

#endif
