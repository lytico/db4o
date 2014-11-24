using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;

namespace LinqTuning
{
	internal class NullSodaQueryFactory : ISodaQueryFactory, IQuery, IObjectSet, IConstraint
	{
		public IQuery Query()
		{
			return this;
		}

		public IConstraint Constrain(object constraint)
		{
			return this;
		}

		public IConstraints Constraints()
		{
			throw new System.NotImplementedException();
		}

		public IQuery Descend(string fieldName)
		{
			return this;
		}

		public IObjectSet Execute()
		{
			return this;
		}

		public IQuery OrderAscending()
		{
			throw new System.NotImplementedException();
		}

		public IQuery OrderDescending()
		{
			throw new System.NotImplementedException();
		}

		public IQuery SortBy(IQueryComparator comparator)
		{
			throw new System.NotImplementedException();
		}

		#region Implementation of IEnumerable

		public IEnumerator GetEnumerator()
		{
			yield break;
		}

		#endregion

		#region Implementation of ICollection

		public void CopyTo(Array array, int index)
		{
			throw new System.NotImplementedException();
		}

		public int Count
		{
			get { throw new System.NotImplementedException(); }
		}

		public object SyncRoot
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsSynchronized
		{
			get { throw new System.NotImplementedException(); }
		}

		#endregion

		#region Implementation of IList

		public int Add(object value)
		{
			throw new System.NotImplementedException();
		}

		public bool Contains(object value)
		{
			throw new System.NotImplementedException();
		}

		public void Clear()
		{
			throw new System.NotImplementedException();
		}

		public int IndexOf(object value)
		{
			throw new System.NotImplementedException();
		}

		public void Insert(int index, object value)
		{
			throw new System.NotImplementedException();
		}

		public void Remove(object value)
		{
			throw new System.NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new System.NotImplementedException();
		}

		public object this[int index]
		{
			get { throw new System.NotImplementedException(); }
			set { throw new System.NotImplementedException(); }
		}

		public bool IsReadOnly
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsFixedSize
		{
			get { throw new System.NotImplementedException(); }
		}

		#endregion

		#region Implementation of IObjectSet

		public IExtObjectSet Ext()
		{
			throw new System.NotImplementedException();
		}

		public bool HasNext()
		{
			throw new System.NotImplementedException();
		}

		public object Next()
		{
			throw new System.NotImplementedException();
		}

		public void Reset()
		{
			throw new System.NotImplementedException();
		}

		#endregion

		#region Implementation of IConstraint

		public IConstraint And(IConstraint with)
		{
			return this;
		}

		public IConstraint Or(IConstraint with)
		{
			return this;
		}

		public IConstraint Equal()
		{
			return this;
		}

		public IConstraint Greater()
		{
			return this;
		}

		public IConstraint Smaller()
		{
			return this;
		}

		public IConstraint Identity()
		{
			throw new System.NotImplementedException();
		}

		public IConstraint ByExample()
		{
			throw new System.NotImplementedException();
		}

		public IConstraint Like()
		{
			throw new System.NotImplementedException();
		}

		public IConstraint Contains()
		{
			throw new System.NotImplementedException();
		}

		public IConstraint StartsWith(bool caseSensitive)
		{
			throw new System.NotImplementedException();
		}

		public IConstraint EndsWith(bool caseSensitive)
		{
			throw new System.NotImplementedException();
		}

		public IConstraint Not()
		{
			return this;
		}

		public object GetObject()
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}