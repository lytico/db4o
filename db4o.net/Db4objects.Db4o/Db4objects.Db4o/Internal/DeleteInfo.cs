/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class DeleteInfo : TreeInt
	{
		internal int _cascade;

		public ObjectReference _reference;

		public DeleteInfo(int id, ObjectReference reference, int cascade) : base(id)
		{
			_reference = reference;
			_cascade = cascade;
		}

		public override object ShallowClone()
		{
			Db4objects.Db4o.Internal.DeleteInfo deleteinfo = new Db4objects.Db4o.Internal.DeleteInfo
				(0, _reference, _cascade);
			return ShallowCloneInternal(deleteinfo);
		}
	}
}
