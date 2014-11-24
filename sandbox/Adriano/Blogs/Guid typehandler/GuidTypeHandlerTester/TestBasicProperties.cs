using System;
using Db4oTestRunner;

namespace GuidTypeHandlerTester
{
	class TestBasicProperties : AbstractDb4oTesterBase
	{
		protected override void Run()
		{
			Db().Store(new GuidItem("foo", Guid.NewGuid()));
			_logger.LogMessage("\r\nHas id: {0}", Db().Ext().StoredClass(typeof(Guid)).HasClassIndex());
		}
	}
}
