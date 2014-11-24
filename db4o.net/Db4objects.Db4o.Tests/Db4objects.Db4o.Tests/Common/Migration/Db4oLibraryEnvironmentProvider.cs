/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Tests.Common.Migration;

namespace Db4objects.Db4o.Tests.Common.Migration
{
	public class Db4oLibraryEnvironmentProvider
	{
		private readonly IDictionary _environments = new Hashtable();

		private readonly Sharpen.IO.File _classPath;

		public Db4oLibraryEnvironmentProvider(Sharpen.IO.File classPath)
		{
			_classPath = classPath;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual Db4oLibraryEnvironment EnvironmentFor(string path)
		{
			Db4oLibraryEnvironment existing = ExistingEnvironment(path);
			if (existing != null)
			{
				return existing;
			}
			return NewEnvironment(path);
		}

		private Db4oLibraryEnvironment ExistingEnvironment(string path)
		{
			return ((Db4oLibraryEnvironment)_environments[path]);
		}

		/// <exception cref="System.IO.IOException"></exception>
		private Db4oLibraryEnvironment NewEnvironment(string path)
		{
			Db4oLibraryEnvironment env = new Db4oLibraryEnvironment(new Sharpen.IO.File(path)
				, _classPath);
			_environments[path] = env;
			return env;
		}

		public virtual void DisposeAll()
		{
			for (IEnumerator eIter = _environments.Values.GetEnumerator(); eIter.MoveNext(); )
			{
				Db4oLibraryEnvironment e = ((Db4oLibraryEnvironment)eIter.Current);
				e.Dispose();
			}
			_environments.Clear();
		}
	}
}
