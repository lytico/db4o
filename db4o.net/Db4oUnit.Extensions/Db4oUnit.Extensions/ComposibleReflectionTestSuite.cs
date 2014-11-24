/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4oUnit.Extensions
{
	public abstract class ComposibleReflectionTestSuite : ReflectionTestSuite
	{
		protected virtual Type[] ComposeTests(Type[] classes)
		{
			return ComposibleTestSuite.Concat(classes, ComposeWith());
		}

		protected virtual Type[] ComposeWith()
		{
			return new Type[0];
		}
	}
}
