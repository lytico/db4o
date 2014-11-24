using Db4oUnit;

namespace Db4oTool.Tests.Core
{
	class ContextVariableTestCase : ITestCase
	{
		public void TestUsing()
		{
			ContextVariable<bool> var = new ContextVariable<bool>(true);
			Assert.AreEqual(true, var.Value);
			bool executedBlock = false;
			var.Using(false, delegate
			{
				executedBlock = true;
         		Assert.AreEqual(false, var.Value);	
         	});
			Assert.IsTrue(executedBlock);
			Assert.AreEqual(true, var.Value);
		}
	}
}
