/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Tests.Common.TA
{
	public class MockActivator : IActivator
	{
		private int _readCount;

		private int _writeCount;

		public MockActivator()
		{
		}

		public virtual int Count()
		{
			return _readCount + _writeCount;
		}

		public virtual void Activate(ActivationPurpose purpose)
		{
			if (purpose == ActivationPurpose.Read)
			{
				++_readCount;
			}
			else
			{
				++_writeCount;
			}
		}

		public virtual int WriteCount()
		{
			return _writeCount;
		}

		public virtual int ReadCount()
		{
			return _readCount;
		}

		public static Db4objects.Db4o.Tests.Common.TA.MockActivator ActivatorFor(IActivatable
			 obj)
		{
			Db4objects.Db4o.Tests.Common.TA.MockActivator activator = new Db4objects.Db4o.Tests.Common.TA.MockActivator
				();
			obj.Bind(activator);
			return activator;
		}
	}
}
