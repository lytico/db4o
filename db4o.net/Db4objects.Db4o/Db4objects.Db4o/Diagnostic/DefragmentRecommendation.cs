/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>Diagnostic to recommend Defragment when needed.</summary>
	/// <remarks>Diagnostic to recommend Defragment when needed.</remarks>
	public class DefragmentRecommendation : DiagnosticBase
	{
		private readonly DefragmentRecommendation.DefragmentRecommendationReason _reason;

		public DefragmentRecommendation(DefragmentRecommendation.DefragmentRecommendationReason
			 reason)
		{
			_reason = reason;
		}

		public class DefragmentRecommendationReason
		{
			internal readonly string _message;

			public DefragmentRecommendationReason(string reason)
			{
				_message = reason;
			}

			public static readonly DefragmentRecommendation.DefragmentRecommendationReason DeleteEmbeded
				 = new DefragmentRecommendation.DefragmentRecommendationReason("Delete Embedded not supported on old file format."
				);
		}

		public override string Problem()
		{
			return "Database file format is old or database is highly fragmented.";
		}

		public override object Reason()
		{
			return _reason._message;
		}

		public override string Solution()
		{
			return "Defragment the database.";
		}
	}
}
