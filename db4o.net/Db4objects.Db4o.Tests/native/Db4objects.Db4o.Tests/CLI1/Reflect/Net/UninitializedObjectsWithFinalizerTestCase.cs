/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */

using System;
using System.Collections.Generic;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.Reflect.Net
{
	public class UninitializedObjectsWithFinalizerTestCase : AbstractDb4oTestCase
	{
#if !CF
		protected override void Store()
		{
			Store(new TestSubject("Test"));

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		public void TestUninitilizedObjects()
		{
			Reopen();
			
			IList <TestSubject> result = Db().Query<TestSubject>();
				
			Assert.AreEqual(1, result.Count);
			Db().Activate(result[0], 2);
			Assert.AreEqual("Test", result[0].name);
		}
	}

	public class TestSubject
	{
		public string name;

		public TestSubject(string _name)
		{
			name = _name;
		}

		~TestSubject()
		{
            // Just access an object method...
            name = name.ToUpper();
		}
#endif
    }
}
