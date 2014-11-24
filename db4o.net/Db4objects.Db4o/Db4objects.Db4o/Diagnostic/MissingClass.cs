/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>Diagnostic if class not found</summary>
	public class MissingClass : DiagnosticBase
	{
		public readonly string _className;

		public MissingClass(string className)
		{
			_className = className;
		}

		public override string Problem()
		{
			return "Class not found in classpath.";
		}

		public override object Reason()
		{
			return _className;
		}

		public override string Solution()
		{
			return "Check your classpath.";
		}
	}
}
