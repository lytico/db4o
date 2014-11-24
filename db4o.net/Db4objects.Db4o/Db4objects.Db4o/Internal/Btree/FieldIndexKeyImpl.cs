/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <summary>
	/// Composite key for field indexes, first compares on the actual
	/// indexed field _value and then on the _parentID (which is a
	/// reference to the containing object).
	/// </summary>
	/// <remarks>
	/// Composite key for field indexes, first compares on the actual
	/// indexed field _value and then on the _parentID (which is a
	/// reference to the containing object).
	/// </remarks>
	/// <exclude></exclude>
	public class FieldIndexKeyImpl : IFieldIndexKey
	{
		private readonly object _value;

		private readonly int _parentID;

		public FieldIndexKeyImpl(int parentID, object value)
		{
			_parentID = parentID;
			_value = value;
		}

		public virtual int ParentID()
		{
			return _parentID;
		}

		public virtual object Value()
		{
			return _value;
		}

		public override string ToString()
		{
			return "FieldIndexKey(" + _parentID + ", " + SafeString(_value) + ")";
		}

		private string SafeString(object value)
		{
			if (null == value)
			{
				return "null";
			}
			return value.ToString();
		}
	}
}
