/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections.Generic;
using Db4objects.Db4o.Activation;
using Sharpen;
using Sharpen.Util;

namespace Db4objects.Db4o.Collections
{
	/// <summary>Transparent activatable ArrayList implementation.
	/// </summary>
	/// <remarks>
	/// Transparent activatable ArrayList implementation. Implements IList
	/// interface using an array to store elements. Each ArrayList4 instance
	/// has a capacity, which indicates the size of the internal array.
	/// <br/>
	/// <br/>
	/// When instantiated as a result of a query, all the internal members
	/// are NOT activated at all. When internal members are required to
	/// perform an operation, the instance transparently activates all the
	/// members.
	/// </remarks>
	/// <seealso cref="System.Collections.ArrayList">System.Collections.ArrayList
	/// </seealso>
	/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
	/// </seealso>
	public partial class ArrayList4<E>
	{
		private E[] elements;

		private int listSize;

		[System.NonSerialized]
		private IActivator _activator;

		/// <summary>activate basic implementation.</summary>
		/// <remarks>activate basic implementation.</remarks>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable</seealso>
		public virtual void Activate(ActivationPurpose purpose)
		{
			if (_activator != null)
			{
				_activator.Activate(purpose);
			}
		}

		/// <summary>bind basic implementation.</summary>
		/// <remarks>bind basic implementation.</remarks>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable</seealso>
		public virtual void Bind(IActivator activator)
		{
			if (_activator == activator)
			{
				return;
			}
			if (activator != null && _activator != null)
			{
				throw new InvalidOperationException();
			}
			_activator = activator;
		}

		/// <summary>
		/// Initializes a new collection with the initial capacity = 10.
		/// </summary>
		public ArrayList4() : this(10)
		{
		}

		/// <summary>
		/// Initializes a collection with the members of the parameter collection.
		/// </summary>
		public ArrayList4(ICollection<E> c)
		{
			E[] data = CollectionToArray(c);
			elements = AllocateStorage(data.Length);
			listSize = data.Length;
			System.Array.Copy(data, 0, elements, 0, data.Length);
		}

		/// <summary>
		/// Initializes a collection of the specified initial capacity.
		/// </summary>
		public ArrayList4(int initialCapacity)
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentException();
			}
			elements = AllocateStorage(initialCapacity);
			listSize = 0;
		}

		/// <summary> Inserts an element into the collection
		/// at the specified index. </summary>
		/// <remarks> Inserts an element into the collection
		/// at the specified index.</remarks>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
		/// </seealso>
		internal virtual void Add(int index, E element)
		{
			CheckIndex(index, 0, Count);
			EnsureCapacity(Count + 1);
			ArrayCopyElements(index, index + 1, listSize - index);
			elements[index] = element;
			IncreaseSize(1);
			MarkModified();
		}

		private void ArrayCopyElements(int sourceIndex, int targetIndex, int length)
		{
			ActivateForWrite();
			System.Array.Copy(elements, sourceIndex, elements, targetIndex, length);
		}

		internal bool AddAllImpl(int index, E[] toBeAdded)
		{
			CheckIndex(index, 0, Count);
			int length = toBeAdded.Length;
			if (length == 0)
			{
				return false;
			}
			EnsureCapacity(Count + length);
			ArrayCopyElements(index, index + length, Count - index);
			System.Array.Copy(toBeAdded, 0, elements, index, length);
			IncreaseSize(length);
			MarkModified();
			return true;
		}

		/// <summary> Removes all elements from the collection.</summary>
		/// <remarks> Removes all elements from the collection.</remarks>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
		/// </seealso>
		public virtual void Clear()
		{
			int size = Count;
			ActivateForWrite();
			Arrays.Fill(elements, 0, size, DefaultValue());
			SetSize(0);
			MarkModified();
		}

		/// <summary> Resizes the collection capacity to the specified size if the
		/// current capacity is less than the parameter value.</summary>
		/// <remarks> Resizes the collection capacity to the specified size if the
		/// current capacity is less than the parameter value.</remarks>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
		/// </seealso>
		public virtual void EnsureCapacity(int minCapacity)
		{
			Activate(ActivationPurpose.Read);
			if (minCapacity <= Capacity())
			{
				return;
			}
			Resize(minCapacity);
		}

		private int Capacity()
		{
			return elements.Length;
		}

		/// <summary> Returns the collection element at the specified index.</summary>
		/// <remarks> Returns the collection element at the specified index.</remarks>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
		/// </seealso>
		public virtual E Get(int index)
		{
			CheckIndex(index, 0, Count - 1);
			return elements[index];
		}

		/// <summary> Removes the collection element at the specified index.</summary>
		/// <remarks> Removes the collection element at the specified index.</remarks>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
		/// </seealso>
		internal virtual E RemoveImpl(int index)
		{
			int size = Count;
			E element = this[index];
			ArrayCopyElements(index + 1, index, size - index - 1);
			elements[size - 1] = DefaultValue();
			DecreaseSize(1);
			MarkModified();
			return element;
		}

		private void RemoveRangeImpl(int fromIndex, int count)
		{
			int size = Count;
			int toIndex = fromIndex + count;
			if ((fromIndex < 0 || fromIndex >= size || toIndex > size || toIndex < fromIndex))
			{
				throw new IndexOutOfRangeException();
			}
			if (count == 0)
			{
				return;
			}
			System.Array.Copy(elements, toIndex, elements, fromIndex, size - toIndex);
			Arrays.Fill(elements, size - count, size, DefaultValue());
			DecreaseSize(count);
			MarkModified();
		}

		/// <summary> Replaces the collection element with the specified object at the specified index.</summary>
		/// <remarks> Replaces the collection element with the specified object at the specified index.</remarks>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
		/// </seealso>
		internal virtual E Set(int index, E element)
		{
			E oldValue = this[index];
			ActivateForWrite();
			elements[index] = element;
			return oldValue;
		}

		/// <summary> Returns the size of the collection.</summary>
		/// <remarks> Returns the size of the collection.</remarks>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
		/// </seealso>
		public virtual int Count
		{
			get
			{
				Activate(ActivationPurpose.Read);
				return listSize;
			}
		}

		/// <summary> Resizes the collection to its actual size.</summary>
		/// <remarks> Resizes the collection to its actual size.</remarks>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
		/// </seealso>
		public virtual void TrimExcess()
		{
			ActivateForWrite();
			Resize(Count);
		}

		private void Resize(int minCapacity)
		{
			MarkModified();
			E[] temp = AllocateStorage(minCapacity);
			System.Array.Copy(elements, 0, temp, 0, Count);
			elements = temp;
		}

		internal virtual void SetSize(int count)
		{
			listSize = count;
		}

		internal virtual void IncreaseSize(int count)
		{
			listSize += count;
		}

		internal virtual void DecreaseSize(int count)
		{
			listSize -= count;
		}

		internal virtual void MarkModified()
		{
			++modCount;
		}

		private void ActivateForWrite()
		{
			Activate(ActivationPurpose.Write);
		}
	}
}
#endif // !SILVERLIGHT
