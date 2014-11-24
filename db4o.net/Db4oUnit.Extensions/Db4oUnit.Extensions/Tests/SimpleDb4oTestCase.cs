/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Tests;
using Db4oUnit.Mocking;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Extensions.Tests
{
	public class SimpleDb4oTestCase : AbstractDb4oTestCase
	{
		public static readonly DynamicVariable RecorderVariable = DynamicVariable.NewInstance
			();

		public class Data
		{
		}

		protected override void Configure(IConfiguration config)
		{
			Record(new MethodCall("fixture", new object[] { Fixture() }));
			Record(new MethodCall("configure", new object[] { config }));
		}

		private void Record(MethodCall call)
		{
			Recorder().Record(call);
		}

		private MethodCallRecorder Recorder()
		{
			return ((MethodCallRecorder)RecorderVariable.Value);
		}

		protected override void Store()
		{
			Record(new MethodCall("store", new object[] {  }));
			Fixture().Db().Store(new SimpleDb4oTestCase.Data());
		}

		public virtual void TestResultSize()
		{
			Record(new MethodCall("testResultSize", new object[] {  }));
			Assert.AreEqual(1, Fixture().Db().QueryByExample(typeof(SimpleDb4oTestCase.Data))
				.Count);
		}
	}
}
