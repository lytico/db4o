/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs;

namespace Db4objects.Drs.Inside
{
	public class ObjectStateImpl : IObjectState
	{
		public const long Unknown = -1;

		private object _object;

		private bool _isNew;

		private bool _wasModified;

		private long _modificationDate;

		public virtual object GetObject()
		{
			return _object;
		}

		public virtual bool IsNew()
		{
			return _isNew;
		}

		public virtual bool WasModified()
		{
			return _wasModified;
		}

		public virtual long ModificationDate()
		{
			return _modificationDate;
		}

		internal virtual void SetAll(object obj, bool isNew, bool wasModified, long modificationDate
			)
		{
			_object = obj;
			_isNew = isNew;
			_wasModified = wasModified;
			_modificationDate = modificationDate;
		}

		public override string ToString()
		{
			return "ObjectStateImpl{" + "_object=" + _object + ", _isNew=" + _isNew + ", _wasModified="
				 + _wasModified + ", _modificationDate=" + _modificationDate + '}';
		}

		public virtual bool IsKnown()
		{
			return _modificationDate != Unknown;
		}
	}
}
