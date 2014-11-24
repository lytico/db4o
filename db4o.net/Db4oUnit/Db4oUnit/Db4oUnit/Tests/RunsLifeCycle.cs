/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Tests;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Tests
{
	public class RunsLifeCycle : ITestCase, ITestLifeCycle
	{
		public static DynamicVariable _tearDownCalled = DynamicVariable.NewInstance();

		private bool _setupCalled = false;

		public virtual void SetUp()
		{
			_setupCalled = true;
		}

		public virtual void TearDown()
		{
			TearDownCalled().value = true;
		}

		public virtual bool SetupCalled()
		{
			return _setupCalled;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestMethod()
		{
			Assert.IsTrue(_setupCalled);
			Assert.IsTrue(!(((bool)TearDownCalled().value)));
			throw FrameworkTestCase.Exception;
		}

		private ByRef TearDownCalled()
		{
			return ((ByRef)_tearDownCalled.Value);
		}
	}
}
