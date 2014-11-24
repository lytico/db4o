using System;
using Db4oTool.Core;
using Mono.Cecil;

namespace Db4oTool.Tests.Core
{
	public class AcceptNoneFilter : ITypeFilter
	{
		public bool Accept(TypeDefinition typeDef)
		{
			return false;
		}
	}

    class ByFilterTestCase : SingleResourceTestCase
	{
		protected override string ResourceName
		{
			get { return "ByFilterInstrumentationSubject"; }
		}

		protected override string CommandLine
		{
			get
			{
				return "-by-filter:Db4oTool.Tests.Core.AcceptNoneFilter,Db4oTool.Tests"
					+ " -instrumentation:Db4oTool.Tests.Core.TraceInstrumentation,Db4oTool.Tests";
			}
		}
	}
}
