/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>
	/// Query tries to descend into a field of a class that is configured to be translated
	/// (and thus cannot be descended into).
	/// </summary>
	/// <remarks>
	/// Query tries to descend into a field of a class that is configured to be translated
	/// (and thus cannot be descended into).
	/// </remarks>
	public class DescendIntoTranslator : DiagnosticBase
	{
		private string className;

		private string fieldName;

		public DescendIntoTranslator(string className_, string fieldName_)
		{
			className = className_;
			fieldName = fieldName_;
		}

		public override string Problem()
		{
			return "Query descends into field(s) of translated class.";
		}

		public override object Reason()
		{
			return className + "." + fieldName;
		}

		public override string Solution()
		{
			return "Consider dropping the translator configuration or resort to evaluations/unoptimized NQs.";
		}
	}
}
