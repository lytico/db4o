/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Mocking;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Tests.Common.TA
{
	public class MockActivatable : IActivatable
	{
		[System.NonSerialized]
		private MethodCallRecorder _recorder;

		public virtual MethodCallRecorder Recorder()
		{
			if (null == _recorder)
			{
				_recorder = new MethodCallRecorder();
			}
			return _recorder;
		}

		public virtual void Bind(IActivator activator)
		{
			Record(new MethodCall("bind", new object[] { activator }));
		}

		public virtual void Activate(ActivationPurpose purpose)
		{
			Record(new MethodCall("activate", new object[] { purpose }));
		}

		private void Record(MethodCall methodCall)
		{
			Recorder().Record(methodCall);
		}
	}
}
