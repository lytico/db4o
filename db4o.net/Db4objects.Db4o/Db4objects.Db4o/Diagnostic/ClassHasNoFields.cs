/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>Diagnostic, if class has no fields.</summary>
	/// <remarks>Diagnostic, if class has no fields.</remarks>
	public class ClassHasNoFields : DiagnosticBase
	{
		private readonly string _className;

		public ClassHasNoFields(string className)
		{
			_className = className;
		}

		public override object Reason()
		{
			return _className;
		}

		public override string Problem()
		{
			return "Class does not contain any persistent fields";
		}

		public override string Solution()
		{
			return "Every class in the hierarchy requires overhead for the maintenance of a class index."
				 + " Consider removing this class from the hierarchy, if it is not needed.";
		}
	}
}
