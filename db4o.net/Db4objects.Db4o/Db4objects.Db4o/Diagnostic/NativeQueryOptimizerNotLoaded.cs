/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	public class NativeQueryOptimizerNotLoaded : DiagnosticBase
	{
		private int _reason;

		private readonly Exception _details;

		public const int NqNotPresent = 1;

		public const int NqConstructionFailed = 2;

		public NativeQueryOptimizerNotLoaded(int reason, Exception details)
		{
			_reason = reason;
			_details = details;
		}

		public override string Problem()
		{
			return "Native Query Optimizer could not be loaded";
		}

		public override object Reason()
		{
			switch (_reason)
			{
				case NqNotPresent:
				{
					return AppendDetails("Native query not present.");
				}

				case NqConstructionFailed:
				{
					return AppendDetails("Native query couldn't be instantiated.");
				}

				default:
				{
					return AppendDetails("Reason not specified.");
					break;
				}
			}
		}

		public override string Solution()
		{
			return "If you to have the native queries optimized, please check that the native query jar is present in the class-path.";
		}

		private object AppendDetails(string reason)
		{
			if (_details == null)
			{
				return reason;
			}
			return reason + "\n" + _details.ToString();
		}
	}
}
