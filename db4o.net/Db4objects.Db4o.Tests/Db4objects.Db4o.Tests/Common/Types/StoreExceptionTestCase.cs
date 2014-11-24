/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Types
{
	public class StoreExceptionTestCase : AbstractDb4oTestCase
	{
		// The following failed with JDK7 before the UnmodifiableListTypeHandler was introduced.
		public virtual void Test()
		{
			Exception e = new Exception();
			Db().Store(e);
		}
	}
}
