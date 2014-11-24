/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Config
{
	/// <summary>
	/// Assigns a fixed, pre-defined name to the given
	/// <see cref="Db4objects.Db4o.IObjectContainer">Db4objects.Db4o.IObjectContainer</see>
	/// .
	/// </summary>
	public class SimpleNameProvider : INameProvider
	{
		private readonly string _name;

		public SimpleNameProvider(string name)
		{
			_name = name;
		}

		public virtual string Name(IObjectContainer db)
		{
			return _name;
		}
	}
}
