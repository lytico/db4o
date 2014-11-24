namespace Db4oTool.Tests.Core
{
	class PreserveDebugInfoTestCase : SingleResourceTestCase
	{
		protected override bool AcceptsReleaseMode
		{
			get { return false; }
		}

		protected override string ResourceName
		{
			get { return "PreserveDebugInfoSubject"; }
		}

		protected override string CommandLine
		{
			get
			{
				return "-debug"
					+ " -instrumentation:Db4oTool.Tests.Core.TraceInstrumentation,Db4oTool.Tests";
			}
		}

		// FIXME: remove this method after updating cecil
		// (waiting for Module.FullLoad patch to be applied)
		protected override void InstrumentAssembly(string path)
		{
			 //base.InstrumentAssembly(path);
		}
	}
}
