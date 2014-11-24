/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class IdObjectCollector
	{
		private TreeInt _ids;

		private List4 _objects;

		public virtual void AddId(int id)
		{
			_ids = (TreeInt)((TreeInt)Tree.Add(_ids, new TreeInt(id)));
		}

		public virtual TreeInt Ids()
		{
			return _ids;
		}

		public virtual void Add(object obj)
		{
			_objects = new List4(_objects, obj);
		}

		public virtual IEnumerator Objects()
		{
			return new Iterator4Impl(_objects);
		}
	}
}
