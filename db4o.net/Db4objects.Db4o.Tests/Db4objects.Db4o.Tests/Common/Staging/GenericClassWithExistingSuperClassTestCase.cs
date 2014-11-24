/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Tests.Common.Assorted;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	public class GenericClassWithExistingSuperClassTestCase : UnavailableClassTestCaseBase
	{
		public class Super
		{
			public int _id;
		}

		public class Sub : GenericClassWithExistingSuperClassTestCase.Super
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new GenericClassWithExistingSuperClassTestCase.Sub());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestFieldAccess()
		{
			ReopenHidingClasses(new Type[] { typeof(GenericClassWithExistingSuperClassTestCase.Sub
				) });
			RetrieveOnlyInstance(typeof(GenericClassWithExistingSuperClassTestCase.Sub));
		}
	}
}
