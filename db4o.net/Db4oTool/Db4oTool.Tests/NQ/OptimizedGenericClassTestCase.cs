using Db4oTool.Tests.Core;

namespace Db4oTool.Tests.NQ
{
	class OptimizedGenericClassTestCase : SingleResourceTestCase
	{
		protected override string CommandLine
		{
			get { return "-nq"; }
		}

		protected override string ResourceName
		{
			get { return "GenericClassTestSubject"; }
		}


	}
}
