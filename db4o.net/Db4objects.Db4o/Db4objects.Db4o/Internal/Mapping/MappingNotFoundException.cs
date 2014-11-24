/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Internal.Mapping
{
	/// <exclude></exclude>
	[System.Serializable]
	public class MappingNotFoundException : Exception
	{
		private const long serialVersionUID = -1771324770287654802L;

		private int _id;

		public MappingNotFoundException(int id)
		{
			this._id = id;
		}

		public virtual int Id()
		{
			return _id;
		}

		public override string ToString()
		{
			return base.ToString() + " : " + _id;
		}
	}
}
