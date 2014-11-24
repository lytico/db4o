/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */

using Sharpen.IO;

namespace Db4objects.Db4o.Tests.Common.Migration
{
	class PathProvider
	{
		public static File TestCasePath()
		{
			return new File(typeof(PathProvider).Module.FullyQualifiedName);
		}
	}
}
