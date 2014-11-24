/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>Diagnostic, if update depth greater than 1.</summary>
	/// <remarks>Diagnostic, if update depth greater than 1.</remarks>
	public class UpdateDepthGreaterOne : DiagnosticBase
	{
		private readonly int _depth;

		public UpdateDepthGreaterOne(int depth)
		{
			_depth = depth;
		}

		public override object Reason()
		{
			return "configuration.common().configure().updateDepth(" + _depth + ")";
		}

		public override string Problem()
		{
			return "A global update depth greater than 1 is not recommended";
		}

		public override string Solution()
		{
			return "Increasing the global update depth to a value greater than 1 is only recommended for"
				 + " testing, not for production use. If individual deep updates are needed, consider using"
				 + " ExtObjectContainer#set(object, depth) and make sure to profile the performance of each call.";
		}
	}
}
