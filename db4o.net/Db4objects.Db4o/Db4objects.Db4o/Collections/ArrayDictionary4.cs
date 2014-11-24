/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;
using Db4objects.Db4o.Activation;
using Sharpen;
using Sharpen.Util;

namespace Db4objects.Db4o.Collections
{
	/// <summary>Transparent activatable IDictionary implementation.
	/// </summary>
	/// <remarks>
	/// Transparent activatable IDictionary implementation. Implements IDictionary interface
	/// using two arrays to store keys and values.
	/// <br/>
	/// <br/>
	/// When instantiated as a result of a query, all the internal members
	/// are NOT activated at all. When internal members are required to
	/// perform an operation, the instance transparently activates all the
	/// members.
	/// </remarks>
	/// <seealso cref="System.Collections.Generic.IDictionary">System.Collections.IDictionary
	/// </seealso>
	/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
	/// </seealso>
	public partial class ArrayDictionary4<K, V>
	{
		private K[] _keys;

		private V[] _values;

		private int _size;

		[System.NonSerialized]
		private IActivator _activator;

		/// <summary>
		/// Initializes a new collection with the initial capacity = 16.
		/// </summary>
		public ArrayDictionary4() : this(16)
		{
		}

		/// <summary>
		/// Initializes a collection of the specified initial capacity.
		/// </summary>
		public ArrayDictionary4(int initialCapacity)
		{
			InitializeBackingArray(initialCapacity);
		}

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

		/// <summary> System.Collections.Generic.IDictionary implementation but transparently activates
		/// the members as required.</summary>
		/// <remarks> System.Collections.Generic.IDictionary implementation but transparently activates
		/// the members as required.</remarks>
		/// <seealso cref="System.Collections.Generic.IDictionary"/>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
		/// </seealso>
		public virtual void Clear()
		{
			Activate(ActivationPurpose.Write);
			_size = 0;
			Arrays.Fill(_keys, DefaultKeyValue());
			Arrays.Fill(_values, DefaultValue());
		}

		private bool ContainsKeyImpl(K key)
		{
			Activate(ActivationPurpose.Read);
			return IndexOfKey(key) != -1;
		}

		private V ValueAt(int index)
		{
			return _values[index];
		}

		private K KeyAt(int i)
		{
			return _keys[i];
		}

		private V Replace(int index, V value)
		{
			V oldValue = ValueAt(index);
			_values[index] = value;
			return oldValue;
		}

		/// <summary> Returns the number of elements in the collection.</summary>
		/// <remarks> Returns the number of elements in the collection. The collection gets activated. </remarks>
		/// <seealso cref="System.Collections.Generic.IDictionary"/>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
		/// </seealso>
		public virtual int Count
		{
			get
			{
				Activate(ActivationPurpose.Read);
				return _size;
			}
		}

		/// <summary> Returns the values of the collection.</summary>
		/// <remarks> Returns the values of the collection. The collection gets activated.</remarks>
		/// <seealso cref="System.Collections.Generic.IDictionary"/>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
		/// </seealso>
		public virtual ICollection<V> Values
		{
			get
			{
				Activate(ActivationPurpose.Read);
				List<V> list = new List<V>();
				for (int i = 0; i < _size; i++)
				{
					list.Add(ValueAt(i));
				}
				return list;
			}
		}

		/// <summary> Returns the hash code of the collection.</summary>
		/// <remarks> Returns the hash code of the collection. Collection members
		/// get activated as required.</remarks>
		/// <seealso cref="System.Collections.Generic.IDictionary"/>
		/// <seealso cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable
		/// </seealso>
		public override int GetHashCode()
		{
			int hashCode = 0;
			foreach (KeyValuePair<K, V> entry in this)
			{
				hashCode += entry.GetHashCode();
			}
			return hashCode;
		}

		private void InitializeBackingArray(int length)
		{
			_keys = AllocateKeyStorage(length);
			_values = AllocateValueStorage(length);
		}

		private void Insert(K key, V value)
		{
			EnsureCapacity();
			_keys[_size] = key;
			_values[_size] = value;
			_size++;
		}

		private void EnsureCapacity()
		{
			if (_size == _keys.Length)
			{
				int count = _keys.Length * 2;
				K[] newKeys = AllocateKeyStorage(count);
				V[] newValues = AllocateValueStorage(count);
				System.Array.Copy(_keys, 0, newKeys, 0, _size);
				System.Array.Copy(_values, 0, newValues, 0, _size);
				_keys = newKeys;
				_values = newValues;
			}
		}

		private V Delete(int index)
		{
			Activate(ActivationPurpose.Write);
			V value = ValueAt(index);
			for (int i = index; i < _size - 1; i++)
			{
				_keys[i] = _keys[i + 1];
				_values[i] = _values[i + 1];
			}
			_size--;
			_keys[_size] = DefaultKeyValue();
			_values[_size] = DefaultValue();
			return value;
		}
	}
}
