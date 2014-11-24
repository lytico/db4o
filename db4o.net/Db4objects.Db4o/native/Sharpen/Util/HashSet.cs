/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sharpen.Util
{   
    public class HashSet : ISet, /* IList required for dRS */ IList
    {   
        private readonly static object _object = new object();

		// FIXME: dRS doesn't like using a dictionary here
#if SILVERLIGHT
		private readonly List<object> _elements = new List<object>();
#else
		private readonly ArrayList _elements = new ArrayList();
#endif

        public HashSet()
        {
        }

        public HashSet(ICollection initialValues)
        {
            AddAll(initialValues);
        }

        public bool Add(object o)
        {
			if (Contains(o)) return false;

			_elements.Add(o);
			return true;
        }

        public bool AddAll(ICollection c)
        {
            bool changed = false;
			foreach (object o in c)
			{
				changed |= Add(o);
			}
        	return changed;
        }

        public void Clear()
        {
            _elements.Clear();
        }

        public bool Contains(object o)
        {
        	return _elements.Contains(o);
        }

        public bool ContainsAll(ICollection c)
        {
            foreach (object o in c)
			{
                if (!Contains(o))
                {
                	return false;
                }
            }
            return true;
        }

        public bool IsEmpty
        {
            get { return _elements.Count == 0; }
        }

        public bool Remove(object o)
        {
			if (!Contains(o)) return false;
            
			_elements.Remove(o);
        	return true;
        }

        public bool RemoveAll(ICollection c)
        {
            bool changed = false;
			foreach (object o in c)
			{
				changed |= Remove(o);
			}
        	return changed;
        }

        public void CopyTo(Array array, int index)
        {
#if SILVERLIGHT
            object[] objectArray = new object[array.Length];
            int idx = 0;
            foreach (var a in array)
            {
                objectArray[idx++] = a;
            }
            _elements.CopyTo(objectArray, index);
#else
			_elements.CopyTo(array, index);
#endif
		}

        public int Count
        {
            get { return _elements.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get
            {
#if SILVERLIGHT
				throw new InvalidOperationException();
#else
            	return _elements.SyncRoot;
#endif
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

    	int IList.Add(object value)
    	{
    		((ISet) this).Add(value);
    		return 0;
    	}

		void IList.Remove(object value)
		{
			((ISet)this).Remove(value);
		}

    	int IList.IndexOf(object value)
    	{
    		throw new NotImplementedException();
    	}

    	void IList.Insert(int index, object value)
    	{
    		throw new NotImplementedException();
    	}

    	void IList.RemoveAt(int index)
    	{
    		throw new NotImplementedException();
    	}

    	object IList.this[int index]
    	{
    		get { throw new NotImplementedException(); }
    		set { throw new NotImplementedException(); }
    	}

    	bool IList.IsReadOnly
    	{
    		get { return false; }
    	}

    	bool IList.IsFixedSize
    	{
    		get { return false; }
    	}
    }
}
