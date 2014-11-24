/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>Diagnostic, if query was required to load candidate set from class index.
	/// 	</summary>
	/// <remarks>Diagnostic, if query was required to load candidate set from class index.
	/// 	</remarks>
	public class LoadedFromClassIndex : DiagnosticBase
	{
		private readonly string _className;

		public LoadedFromClassIndex(string className)
		{
			_className = className;
		}

		public override object Reason()
		{
			return _className;
		}

		public override string Problem()
		{
			return "Query candidate set could not be loaded from a field index";
		}

		public override string Solution()
		{
			return "Consider indexing fields that you query for: " + "configuration.common().objectClass("
				 + _className + ").objectField([fieldName]).indexed(true)";
		}
	}
}
