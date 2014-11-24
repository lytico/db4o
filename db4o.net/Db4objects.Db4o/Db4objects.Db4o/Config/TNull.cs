/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Config
{
	/// <exclude></exclude>
	public class TNull : IObjectTranslator
	{
		public virtual object OnStore(IObjectContainer con, object @object)
		{
			return null;
		}

		public virtual void OnActivate(IObjectContainer con, object @object, object members
			)
		{
		}

		public virtual Type StoredClass()
		{
			return typeof(object);
		}
	}
}
