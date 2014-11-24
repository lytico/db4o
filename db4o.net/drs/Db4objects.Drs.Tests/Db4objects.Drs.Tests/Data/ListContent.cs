/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs.Tests.Data
{
	public class ListContent
	{
		private string name;

		public ListContent()
		{
		}

		public ListContent(string name)
		{
			this.name = name;
		}

		public virtual string GetName()
		{
			return name;
		}

		public virtual void SetName(string name)
		{
			this.name = name;
		}

		public override string ToString()
		{
			return "name = " + name;
		}
	}
}
