/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>Diagnostic, if Native Query can not be run optimized.</summary>
	/// <remarks>Diagnostic, if Native Query can not be run optimized.</remarks>
	public class NativeQueryNotOptimized : DiagnosticBase
	{
		private readonly Predicate _predicate;

		private readonly Exception _details;

		public NativeQueryNotOptimized(Predicate predicate, Exception details)
		{
			_predicate = predicate;
			_details = details;
		}

		public override object Reason()
		{
			if (_details == null)
			{
				return _predicate;
			}
			return _predicate != null ? _predicate.ToString() : string.Empty + "\n" + _details
				.Message;
		}

		public override string Problem()
		{
			return "Native Query Predicate could not be run optimized";
		}

		public override string Solution()
		{
			return "This Native Query was run by instantiating all objects of the candidate class. "
				 + "Consider simplifying the expression in the Native Query method. If you feel that "
				 + "the Native Query processor should understand your code better, you are invited to "
				 + "post yout query code to db4o forums at http://developer.db4o.com/forums";
		}
	}
}
