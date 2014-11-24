/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.Internal.Query
{
	/// <summary>
	/// List based objectSet implementation
	/// </summary>
	/// <exclude />
	public class ObjectSetFacade : IExtObjectSet, System.Collections.IList
	{
		public readonly StatefulQueryResult _delegate;

        public ObjectSetFacade(IQueryResult qr)
		{
            _delegate = new StatefulQueryResult(qr);
		}

		void IExtObjectSet.Skip(int count)
		{
			_delegate.Skip(count);
		}

		#region IObjectSet Members
		
		public Object Get(int index)
		{
            return _delegate.Get(index);
        }

		public void Sort(Db4objects.Db4o.Query.IQueryComparator cmp)
		{
			_delegate.Sort(cmp);
		}

		public long[] GetIDs() 
		{
			return _delegate.GetIDs();
		}

		public IExtObjectSet Ext() 
		{
			return this;
		}

		public bool MoveNext()
		{
			return Enumerator().MoveNext();
		}

		private IEnumerator _enumerator;

		private IEnumerator Enumerator()
		{
			if (null == _enumerator)
			{
				_enumerator = GetEnumerator();
			}
			return _enumerator;
		}

		public object Current
		{
			get { return Enumerator().Current; }
		}

		public bool HasNext() 
		{
			return _delegate.HasNext();
		}

		public Object Next() 
		{
			return _delegate.Next();
		}

		public void Reset() 
		{
			_delegate.Reset();
		}

		public int Size() 
		{
			return _delegate.Size();
		}
    
		private Object Lock()
		{
			return _delegate.Lock();
		}
    
		private IObjectContainer ObjectContainer()
		{
			return _delegate.ObjectContainer();
		}

		public StatefulQueryResult Delegate_()
		{
			return _delegate;
		}
		#endregion

		#region IList Members

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public object this[int index]
		{
			get
			{
				return _delegate.Get(index);
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		public void Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		public void Remove(object value)
		{
			throw new NotSupportedException();
		}

		public bool Contains(object value)
		{
			return IndexOf(value) >= 0;
		}

		public void Clear()
		{
			throw new NotSupportedException();
		}

		public int IndexOf(object value)
		{
			return _delegate.IndexOf(value);
		}

		public int Add(object value)
		{
			throw new NotSupportedException();
		}

		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		#endregion

		#region ICollection Members
		public bool IsSynchronized
		{
			get
			{
				return true;
			}
		}

		public int Count
		{
			get
			{
				return Size();
			}
		}

        public void CopyTo(Array array, int index)
        {
            lock (Lock())
            {
                int i = 0;
                int s = _delegate.Size();
                while (i < s)
                {
                    array.SetValue(_delegate.Get(i), index + i);
                    i++;
                }
            }
        }

        public object SyncRoot
		{
			get
			{
				return Lock();
			}
		}

		#endregion
		
		public System.Collections.IEnumerator GetEnumerator()
		{
			IEnumerator enumerator = _delegate.GetEnumerator();
			object current;
			while (MoveNext(enumerator, out current))
			{
				yield return current;
			}
		}

		private bool MoveNext(IEnumerator enumerator, out object current)
		{
			lock (_delegate.Lock())
			{
				if (enumerator.MoveNext())
				{
					current = enumerator.Current;
					return true;
				}
			}
			current = null;
			return false;
		}
	}
}


