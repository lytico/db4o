/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Migration;

namespace Db4objects.Db4o.Tests.Common.Migration
{
	public class Db4oLibrary
	{
		public readonly string path;

		public readonly Db4oLibraryEnvironment environment;

		public Db4oLibrary(string path, Db4oLibraryEnvironment environment)
		{
			this.path = path;
			this.environment = environment;
		}
	}
}
