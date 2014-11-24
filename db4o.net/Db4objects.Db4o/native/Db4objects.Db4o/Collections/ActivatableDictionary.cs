/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;
using System.Collections;
using System.Collections.Generic;

#if !CF && !SILVERLIGHT
using System.Reflection;
using System.Runtime.Serialization;
#endif

using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Collections
{
	[Serializable]
	public class ActivatableDictionary<TKey, TValue> : 
									ActivatableBase,
									IDictionary<TKey, TValue>,
									IDictionary
#if !CF	&& !SILVERLIGHT							
									,ISerializable
									,IDeserializationCallback
#endif
	{
		public ActivatableDictionary()
		{
			_dictionary = new Dictionary<TKey, TValue>();
		}

		public ActivatableDictionary(IEqualityComparer<TKey> comparer)
		{
			_dictionary = new Dictionary<TKey, TValue>(comparer);
		}

		public ActivatableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		{
			_dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
		}

		public ActivatableDictionary(IDictionary<TKey, TValue> dictionary)
		{
			_dictionary = new Dictionary<TKey, TValue>(dictionary);
		}

		public ActivatableDictionary(int capacity)
		{
			_dictionary = new Dictionary<TKey, TValue>(capacity);
		}

		public ActivatableDictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			_dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
		}

#if !CF && !SILVERLIGHT
		protected ActivatableDictionary(SerializationInfo info, StreamingContext context)
		{
			Type type = typeof(Dictionary<TKey, TValue>);
			ConstructorInfo ctor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);

			_dictionary = (IDictionary<TKey, TValue>)ctor.Invoke(new object[] { info, context });
		}
#endif

		#region Implementation of IEnumerable

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			ActivateForRead();
			return _dictionary.GetEnumerator();
		}

		public void Remove(object key)
		{
			Remove((TKey) key);
		}

		object IDictionary.this[object key]
		{
			get { return this[(TKey) key]; }
			set { this[(TKey) key] =  (TValue) value; }
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Implementation of ICollection<KeyValuePair<TKey,TValue>>

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			ActivateForWrite();
			_dictionary.Add(item);
		}

		public bool Contains(object key)
		{
			return ContainsKey( (TKey) key);
		}

		public void Add(object key, object value)
		{
			Add( (TKey) key, (TValue) value);
		}

		public void Clear()
		{
			ActivateForWrite();
			_dictionary.Clear();
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			ActivateForRead();
			return Cast<IDictionary>().GetEnumerator();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			ActivateForRead();
			return _dictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			ActivateForRead();
			_dictionary.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			ActivateForWrite();
			return _dictionary.Remove(item);
		}

		public void CopyTo(Array array, int index)
		{
			ActivateForRead();
			Cast<IDictionary>().CopyTo(array, index);
		}

		public int Count
		{
			get
			{
				ActivateForRead();
				return _dictionary.Count;
			}
		}

		public object SyncRoot
		{
			get
			{
				return Cast<ICollection>().SyncRoot;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return Cast<ICollection>().IsSynchronized;
			}
		}

		ICollection IDictionary.Values
		{
			get { return Values; }
		}

		public bool IsReadOnly
		{
			get
			{
				return _dictionary.IsReadOnly;
			}
		}

		public bool IsFixedSize
		{
			get
			{
				return Cast<IDictionary>().IsFixedSize;
			}
		}

		#endregion

		#region Implementation of IDictionary<TKey,TValue>

		public bool ContainsKey(TKey key)
		{
			ActivateForRead();
			return _dictionary.ContainsKey(key);
		}

		public void Add(TKey key, TValue value)
		{
			ActivateForWrite();
			_dictionary.Add(key, value);
		}

		public bool Remove(TKey key)
		{
			ActivateForWrite();
			return _dictionary.Remove(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			ActivateForRead();
			return _dictionary.TryGetValue(key, out value);
		}

		public TValue this[TKey key]
		{
			get
			{
				ActivateForRead();
				return _dictionary[key];
			}

			set
			{
				ActivateForWrite();
				_dictionary[key] = value;
			}
		}

		public ICollection<TKey> Keys
		{
			get
			{
				ActivateForRead();
				return _dictionary.Keys;
			}
		}

		ICollection IDictionary.Keys
		{
			get
			{
				ActivateForRead();
				return Cast<IDictionary>().Keys;
			}
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				ActivateForRead();
				return _dictionary.Values;
			}
		}

		#endregion

		#region Dictionary<TKey, TValue> methods

		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				ActivateForRead();
				return ((Dictionary<TKey, TValue>) _dictionary).Values;
			}
		}

		public bool ContainsValue(TValue value)
		{
			ActivateForRead();
			return Cast<Dictionary<TKey, TValue>>().ContainsValue(value);
		}

		public IEqualityComparer<TKey> Comparer
		{
			get
			{
				return Cast<Dictionary<TKey, TValue>>().Comparer;
			}
		}

		#endregion

#if !CF && !SILVERLIGHT
		#region Implementation of ISerializable

#if NET_4_0
		[System.Security.SecurityCritical]
#endif
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			ActivateForRead();
			Cast<ISerializable>().GetObjectData(info, context);
		}

		#endregion

		#region Implementation of IDeserializationCallback

		public void OnDeserialization(object sender)
		{
			Cast<IDeserializationCallback>().OnDeserialization(sender);
		}

		#endregion
#endif

		private T Cast<T>()
		{
			return (T)_dictionary;
		}

		private readonly IDictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
	}
}
