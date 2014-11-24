using System.Reflection;
using Db4oTool.Tests.Core;

namespace Db4oTool.Tests.TA
{
	class TANonStorableTypeTestCase : SingleResourceTestCase
	{
		protected override string ResourceName
		{
			get { return "TAUnsafeInstrumentationSubject"; }
		}

		protected override string CommandLine
		{
			get { return "-ta -vv"; }
		}

		protected override string EmitAssemblyFromResource(string resource, Assembly[] references)
		{
			string returnValue = null;
			CompilationServices.Unsafe.Using(true, delegate
           	{
           		returnValue = BaseEmitAssemblyFromResource(resource, references);
           	});
			return returnValue;
		}

		protected override bool ShouldVerify(string resource)
		{
			return false;
		}

		private string BaseEmitAssemblyFromResource(string resource, Assembly[] references)
		{
			return base.EmitAssemblyFromResource(resource, references);
		}
	}
}
