namespace Db4oTool.Tests.Core
{
    class ByAttributeTestCase : SingleResourceTestCase
	{
		protected override string ResourceName
		{
			get { return "ByAttributeInstrumentationSubject"; }
		}

		protected override string CommandLine
		{
			get
			{
				return "-by-attribute:MyCustomAttribute -instrumentation:Db4oTool.Tests.Core.TraceInstrumentation,Db4oTool.Tests";
			}
		}
	}
}
