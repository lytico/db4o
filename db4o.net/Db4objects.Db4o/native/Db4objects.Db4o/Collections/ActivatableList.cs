/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Collections
{
	public class ActivatableList<T> : ActivatableBase, IList<T>, IActivatableCollection<T>
	{
		public ActivatableList()
		{
		}

		public ActivatableList(IEnumerable<T> source)
		{
			_list = new List<T>(source);
		}

		public ActivatableList(int capacity)
		{
			_list = new List<T>(capacity);
		}

		public ReadOnlyCollection<T> AsReadOnly()
		{
			ActivateForRead();
			return AsList().AsReadOnly();
		}

		public IEnumerator<T> GetEnumerator()
		{			
			Activate(ActivationPurpose.Read);
			return AsList().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(T item)
		{
			ActivateForWrite();
			AsList().Add(item);
		}

		public void AddRange(IEnumerable<T> collection)
		{
			ActivateForWrite();
			AsList().AddRange(collection);
		}

		public int BinarySearch(T item)
		{
			ActivateForRead();
			return AsList().BinarySearch(item);
		}

		public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
		{
			ActivateForRead();
			return AsList().BinarySearch(index, count, item, comparer);
		}

		public int BinarySearch(T item, IComparer<T> comparer)
		{
			ActivateForRead();
			return AsList().BinarySearch(item, comparer);
		}

		public void Clear()
		{
			ActivateForWrite();
			AsList().Clear();
		}

		public bool Contains(T item)
		{
			ActivateForRead();
			return AsList().Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			ActivateForRead();
			AsList().CopyTo(array, arrayIndex);
		}

		public void CopyTo(T[] array)
		{
			ActivateForRead();
			AsList().CopyTo(array);
		}

		public void CopyTo(int index, T[] array, int arrayIndex, int count)
		{
			ActivateForRead();
			AsList().CopyTo(index, array, arrayIndex, count);
		}

		public override bool Equals(object obj)
		{
			ActivateForRead();
			return AsList().Equals(obj);
		}

#if !SILVERLIGHT
		public bool Exists(Predicate<T> match)
		{
			ActivateForRead();
			return AsList().Exists(match);
		}

		public T Find(Predicate<T> match)
		{
			ActivateForRead();
			return AsList().Find(match);
		}

		public List<T> FindAll(Predicate<T> match)
		{
			ActivateForRead();
			return AsList().FindAll(match);
		}

		public int FindIndex(Predicate<T> match)
		{
			ActivateForRead();
			return AsList().FindIndex(match);
		}
		
		public int FindIndex(int startIndex, Predicate<T> match)
		{
			ActivateForRead();
			return AsList().FindIndex(startIndex, match);
		}
		
		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			ActivateForRead();
			return AsList().FindIndex(startIndex, count, match);
		}
		
		public T FindLast(Predicate<T> match)
		{
			ActivateForRead();
			return AsList().FindLast(match);
		}
		
		public int FindLastIndex(Predicate<T> match)
		{
			ActivateForRead();
			return AsList().FindLastIndex(match);
		}
		
		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			ActivateForRead();
			return AsList().FindLastIndex(startIndex, match);
		}
		
		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			ActivateForRead();
			return AsList().FindLastIndex(startIndex, count, match);
		}
#endif

		public void ForEach(Action<T> action)
		{
			ActivateForRead();
			AsList().ForEach(action);
		}

		public List<T> GetRange(int index, int count)
		{
			ActivateForRead();
			return AsList().GetRange(index, count);
		}

		public int IndexOf(T item)
		{
			ActivateForRead();
			return AsList().IndexOf(item);
		}

		public int IndexOf(T item, int index)
		{
			ActivateForRead();
			return AsList().IndexOf(item, index);
		}

		public int IndexOf(T item, int index, int count)
		{
			ActivateForRead();
			return AsList().IndexOf(item, index, count);
		}

		public void InsertRange(int index, IEnumerable<T> collection)
		{
			ActivateForWrite();
			AsList().InsertRange(index, collection);
		}

		public int LastIndexOf(T item)
		{
			ActivateForRead();
			return AsList().LastIndexOf(item);
		}

		public int LastIndexOf(T item, int index)
		{
			ActivateForRead();
			return AsList().LastIndexOf(item, index);
		}

		public int LastIndexOf(T item, int index, int count)
		{
			ActivateForRead();
			return AsList().LastIndexOf(item, index, count);
		}

#if !SILVERLIGHT
		public int RemoveAll(Predicate<T> match)
		{
			ActivateForWrite();
			return AsList().RemoveAll(match);
		}
#endif

		public void RemoveRange(int index, int count)
		{
			ActivateForWrite();
			AsList().RemoveRange(index, count);
		}

		public void Reverse()
		{
			ActivateForWrite();
			AsList().Reverse();
		}

		public void Reverse(int index, int count)
		{
			ActivateForWrite();
			AsList().Reverse(index, count);
		}

		public void Sort()
		{
			ActivateForWrite();
			AsList().Sort();
		}

		public void Sort(IComparer<T> comparer)
		{
			ActivateForWrite();
			AsList().Sort(comparer);
		}

		public void Sort(int index, int count, IComparer<T> comparer)
		{
			ActivateForWrite();
			AsList().Sort(index, count, comparer);
		}

		public void Sort(Comparison<T> comparison)
		{
			ActivateForWrite();
			AsList().Sort(comparison);
		}

		public T[] ToArray()
		{
			ActivateForRead();
			return AsList().ToArray();
		}

		public void TrimExcess()
		{
			ActivateForWrite();
			AsList().TrimExcess();
		}

#if !SILVERLIGHT
		public bool TrueForAll(Predicate<T> match)
		{
			ActivateForRead();
			return AsList().TrueForAll(match);
		}

		public List<TOutput> ConvertAll<TOutput>(Converter<T,TOutput> converter)
		{
			ActivateForRead();
			return AsList().ConvertAll(converter);
		}
#endif
        
		public bool Remove(T item)
		{
			ActivateForWrite();
			return AsList().Remove(item);
		}

		public int Count
		{
			get
			{
				ActivateForRead();
				return AsList().Count;
			}
		}

		public int Capacity
		{
			get
			{
				ActivateForRead();
				return AsList().Capacity;
			}

			set
			{
				ActivateForWrite();
				AsList().Capacity = value;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				ActivateForRead();
				return AsIList().IsReadOnly;
			}
		}

		public void Insert(int index, T item)
		{
			ActivateForWrite();
			AsList().Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			ActivateForWrite();
			AsList().RemoveAt(index);
		}

		public T this[int index]
		{
			get
			{
				ActivateForRead();
				return AsList()[index];
			}

			set
			{
				ActivateForWrite();
				AsList()[index] = value;
			}
		}

		private List<T> AsList()
		{
			if (_list == null)
			{
				_list = new List<T>();
			}

			return _list;
		}

		private IList<T> AsIList()
		{
			return AsList();
		}

		private List<T> _list;
	}
}
