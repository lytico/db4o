#if !SILVERLIGHT
using Db4objects.Db4o.Collections;
using Db4objects.Db4o.Tests.Common.TA;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Collections
{
	class ArrayList4ActivatableTestCase : ITestCase
	{
		public void TestCopyTo()
		{
			ArrayList4<int> list = new ArrayList4<int>();
			MockActivator activator = MockActivator.ActivatorFor(list);
			list.CopyTo(new int[1], 0);
			Assert.AreEqual(1, activator.ReadCount());
		}
	}
}
#endif