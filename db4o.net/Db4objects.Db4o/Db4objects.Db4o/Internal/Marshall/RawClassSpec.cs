/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class RawClassSpec
	{
		private readonly string _name;

		private readonly int _superClassID;

		private readonly int _numFields;

		public RawClassSpec(string name, int superClassID, int numFields)
		{
			_name = name;
			_superClassID = superClassID;
			_numFields = numFields;
		}

		public virtual string Name()
		{
			return _name;
		}

		public virtual int SuperClassID()
		{
			return _superClassID;
		}

		public virtual int NumFields()
		{
			return _numFields;
		}
	}
}
