using Db4objects.Db4o;
using Db4oUnit;

namespace Db4oTool.Tests.Core
{
	public class InstrumentedTestCase : ITestLifeCycle
	{
		protected IObjectContainer _container;
		
		public IObjectContainer Container
		{
			set { _container = value; }
			get { return _container; }
		}
		
		public virtual void SetUp()
		{	
		}
		
		public virtual void TearDown()
		{	
		}
	}
}