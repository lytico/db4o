/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>Diagnostic on failed delete.</summary>
	/// <remarks>Diagnostic on failed delete.</remarks>
	public class DeletionFailed : DiagnosticBase
	{
		public override string Problem()
		{
			return "Cascading delete to members failed. Possible reasons: db4o engine updates, corruption, changed class hierarchies.";
		}

		public override object Reason()
		{
			return string.Empty;
		}

		public override string Solution()
		{
			return "Running Defragment may fix.";
		}
	}
}
