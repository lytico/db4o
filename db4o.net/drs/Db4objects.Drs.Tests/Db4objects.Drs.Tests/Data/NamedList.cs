/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests.Data
{
	public class NamedList : DelegatingList
	{
		private string _name;

		public NamedList() : this(null)
		{
		}

		public NamedList(string name) : base(new ArrayList())
		{
			_name = name;
		}

		public virtual string Name()
		{
			return _name;
		}
	}
}
