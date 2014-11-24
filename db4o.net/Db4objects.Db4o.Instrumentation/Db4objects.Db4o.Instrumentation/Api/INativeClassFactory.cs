/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Instrumentation.Api
{
	/// <exclude></exclude>
	public interface INativeClassFactory
	{
		/// <exception cref="System.TypeLoadException"></exception>
		Type ForName(string className);
	}
}
