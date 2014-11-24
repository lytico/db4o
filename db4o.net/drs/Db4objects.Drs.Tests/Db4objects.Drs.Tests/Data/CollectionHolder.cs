/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Sharpen.Util;

namespace Db4objects.Drs.Tests.Data
{
	public class CollectionHolder
	{
		private string name;

		private IDictionary map;

		private IList list;

		private Sharpen.Util.ISet set;

		public CollectionHolder(string name, IDictionary theMap, Sharpen.Util.ISet theSet
			, IList theList)
		{
			this.name = name;
			map = theMap;
			set = theSet;
			list = theList;
		}

		public CollectionHolder() : this("HashMap", new Hashtable(), new HashSet(), new ArrayList
			())
		{
		}

		public CollectionHolder(string name) : this()
		{
			this.name = name;
		}

		public override string ToString()
		{
			return name + ", hashcode = " + GetHashCode();
		}

		public virtual void Map(IDictionary map)
		{
			this.map = map;
		}

		public virtual IDictionary Map()
		{
			return map;
		}

		public virtual void List(IList list)
		{
			this.list = list;
		}

		public virtual IList List()
		{
			return list;
		}

		public virtual void Set(Sharpen.Util.ISet set)
		{
			this.set = set;
		}

		public virtual Sharpen.Util.ISet Set()
		{
			return set;
		}

		public virtual void Name(string name)
		{
			this.name = name;
		}

		public virtual string Name()
		{
			return name;
		}
	}
}
