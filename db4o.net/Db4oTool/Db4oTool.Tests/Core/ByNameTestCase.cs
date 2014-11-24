namespace Db4oTool.Tests.Core
{
    class ByNameTestCase : SingleResourceTestCase
	{
		protected override string ResourceName
		{
			get { return "ByNameInstrumentationSubject"; }
		}

		protected override string CommandLine
		{
			get
			{
				return "-by-name:F.+o -instrumentation:Db4oTool.Tests.Core.TraceInstrumentation,Db4oTool.Tests";
			}
		}
	}
}
