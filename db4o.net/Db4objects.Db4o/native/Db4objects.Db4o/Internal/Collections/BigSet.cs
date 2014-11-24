/* Copyright (C) 2010 Versant Inc.  http://www.db4o.com */
using System.Collections;
using System.Collections.Generic;

namespace Db4objects.Db4o.Internal.Collections
{
	public partial class BigSet<E>
	{
		#region Implementation of ICollection

		void ICollection<E>.Add(E item)
		{
			Add(item);
		}

		bool ICollection<E>.Contains(E item)
		{
			return Contains(item);
		}

		void ICollection<E>.CopyTo(E[] array, int arrayIndex)
		{
			throw new System.NotImplementedException();
		}

		bool ICollection<E>.Remove(E item)
		{
			return Remove(item);
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		IEnumerator<E> IEnumerable<E>.GetEnumerator()
		{
			IEnumerator iterator = BTreeIterator();
			while (iterator.MoveNext())
			{
				yield return (E)Element((int) iterator.Current);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<E>)this).GetEnumerator();
		}

		#endregion

		#region Implementation of ISet<E>

		bool Db4o.Collections.ISet<E>.RemoveAll(IEnumerable<E> es)
		{
			bool result = false;
			foreach (E e in es)
			{
				if (Remove(e))
				{
					result = true;
				}
			}
			return result;
		}

		bool Db4o.Collections.ISet<E>.ContainsAll(IEnumerable<E> es)
		{
			foreach (E e in es)
			{
				if (!Contains(e))
				{
					return false;
				}
			}
			return true;
		}

		#endregion
	}
}
