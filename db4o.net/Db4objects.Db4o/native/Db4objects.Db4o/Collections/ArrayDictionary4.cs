using System;
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Collections
{
    [Obsolete("Use Db4objects.Db4o.Collections.ActivatableDictionary instead")]
	public partial class ArrayDictionary4<K, V> : IDictionary<K, V>, IActivatable
	{	
		public bool IsReadOnly
		{
			get { return false;  }
		}

		void ICollection<KeyValuePair<K, V>>.Add(KeyValuePair<K, V> item)
		{
			Add(item.Key, item.Value);
		}

		public void Add(K key, V value)
		{
            Activate(ActivationPurpose.Read);
            int index = IndexOfKey(key);
            if (index != -1)
            {
                throw new ArgumentException(string.Format("Key {0} already exists", key));
            }
            Activate(ActivationPurpose.Write);
            Insert(key, value);
		}

		public bool Remove(K key)
		{
			Activate(ActivationPurpose.Read);
            int index = IndexOfKey(key);
            if (index == -1) return false;

            Delete(index);
            return true;
		}

		bool ICollection<KeyValuePair<K, V>>.Contains(KeyValuePair<K, V> pair)
		{
			Activate(ActivationPurpose.Read);
            int index = IndexOfKey(pair.Key);
            if (index == -1) return false;

            KeyValuePair<K, V> thisKeyValuePair = new KeyValuePair<K, V>(pair.Key, ValueAt(index));
            return EqualityComparer<KeyValuePair<K, V>>.Default.Equals(thisKeyValuePair, pair);
		}

		void ICollection<KeyValuePair<K, V>>.CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
		{	
            if (array == null) throw new ArgumentNullException();
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException();
            if (arrayIndex >= array.Length || Count > (array.Length - arrayIndex)) throw new ArgumentException();

            for (int i = 0; i < Count; i++)
            {
                KeyValuePair<K, V> keyValuePair = new KeyValuePair<K, V>(KeyAt(i), ValueAt(i));
                array[arrayIndex + i] = keyValuePair;
            }
		}

		bool ICollection<KeyValuePair<K, V>>.Remove(KeyValuePair<K, V> pair)
		{
            if (!((ICollection<KeyValuePair<K, V>>)this).Contains(pair)) return false;

			return Remove(pair.Key);
		}

		public bool TryGetValue(K key, out V value)
		{
			Activate(ActivationPurpose.Read);
			int index = IndexOfKey(key);
			if (index == -1)
			{
				value = default(V);
				return false;
			}
			value = ValueAt(index);
			return true;
		}

		public V this[K key]
		{
            get
            {
				Activate(ActivationPurpose.Read);
                int index = IndexOfKey(key);
                if (index == -1) throw new KeyNotFoundException();
                return ValueAt(index);
            }
            set
            {	
				Activate(ActivationPurpose.Read);
                int index = IndexOfKey(key);
                if (index == -1)
                {
                    Add(key, value);
                }
                else
                {
					Activate(ActivationPurpose.Write);
                    Replace(index, value);
                }
            }
		}

		public ICollection<K> Keys
		{
            get
            {
				Activate(ActivationPurpose.Read);
                K[] keys = new K[_size];
                Array.Copy(_keys, keys, _size);
                return keys;
            }
		}

		public bool ContainsKey(K key)
		{
			return ContainsKeyImpl(key);
		}

		public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
		{
			Activate(ActivationPurpose.Read);
			for (int i = 0; i < _size; ++i)
			{
				yield return new KeyValuePair<K, V>(KeyAt(i), ValueAt(i));
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<K, V>>)this).GetEnumerator();
		}

        private int IndexOfKey(K key)
        {
            if (key == null) throw new ArgumentNullException();
            return Array.IndexOf(_keys, key);
        }
        
        #region Sharpen Helpers
        private static K DefaultKeyValue()
        {
            return default(K);
        }

        private static V DefaultValue()
        {
            return default(V);    
        }
                
        private static K[] AllocateKeyStorage(int length)
        {
            return new K[length];
        }

        private static V[] AllocateValueStorage(int length)
        {
            return new V[length];
        }

        #endregion
    }
}
