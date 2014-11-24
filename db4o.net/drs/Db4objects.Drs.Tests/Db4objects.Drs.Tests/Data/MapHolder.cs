/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;

namespace Db4objects.Drs.Tests.Data
{
	public class MapHolder
	{
		private string name;

		private IDictionary map;

		public MapHolder()
		{
		}

		public MapHolder(string name)
		{
			this.name = name;
			this.map = new Hashtable();
		}

		public virtual string GetName()
		{
			return name;
		}

		public virtual void SetName(string name)
		{
			this.name = name;
		}

		public virtual IDictionary GetMap()
		{
			return map;
		}

		public virtual void SetMap(IDictionary map)
		{
			this.map = map;
		}

		public virtual void Put(object key, object value)
		{
			map[key] = value;
		}

		public override string ToString()
		{
			return "name = " + name + ", map = " + map;
		}
	}
}
