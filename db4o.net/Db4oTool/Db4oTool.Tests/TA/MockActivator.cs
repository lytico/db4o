using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4oTool.Tests.TA
{
	public class MockActivator : IActivator
	{
		public static MockActivator ActivatorFor(object obj)
		{
			MockActivator activator = new MockActivator();
			((IActivatable) obj).Bind(activator);
			return activator;
		}

		private int _readCount;

		private int _writeCount;

		public int ReadCount
		{
			get { return _readCount; }
		}

		public int WriteCount
		{
			get { return _writeCount;  }
		}

		public void Activate(ActivationPurpose purpose)
		{
			if (ActivationPurpose.Read == purpose)
			{
				++_readCount;
			}
			else
			{
				++_writeCount;
			}
		}
	}


}
