/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Types;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <summary>Query Evaluator - Represents such things as &gt;, &gt;=, &lt;, &lt;=, EQUAL, LIKE, etc.
	/// 	</summary>
	/// <remarks>Query Evaluator - Represents such things as &gt;, &gt;=, &lt;, &lt;=, EQUAL, LIKE, etc.
	/// 	</remarks>
	/// <exclude></exclude>
	public class QE : IUnversioned
	{
		internal static readonly QE Default = new QE();

		public const int Nulls = 0;

		public const int Smaller = 1;

		public const int Equal = 2;

		public const int Greater = 3;

		internal virtual QE Add(QE evaluator)
		{
			return evaluator;
		}

		public virtual bool Identity()
		{
			return false;
		}

		internal virtual bool IsDefault()
		{
			return true;
		}

		internal virtual bool Evaluate(QConObject constraint, IInternalCandidate candidate
			, object obj)
		{
			IPreparedComparison prepareComparison = constraint.PrepareComparison(candidate);
			if (obj == null)
			{
				return prepareComparison is Null;
			}
			if (prepareComparison is PreparedArrayContainsComparison)
			{
				return ((PreparedArrayContainsComparison)prepareComparison).IsEqual(obj);
			}
			return prepareComparison.CompareTo(obj) == 0;
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj.GetType() == this.GetType();
		}

		public override int GetHashCode()
		{
			return GetType().GetHashCode();
		}

		// overridden in QENot 
		internal virtual bool Not(bool res)
		{
			return res;
		}

		/// <summary>Specifies which part of the index to take.</summary>
		/// <remarks>
		/// Specifies which part of the index to take.
		/// Array elements:
		/// [0] - smaller
		/// [1] - equal
		/// [2] - greater
		/// [3] - nulls
		/// </remarks>
		/// <param name="bits"></param>
		public virtual void IndexBitMap(bool[] bits)
		{
			bits[QE.Equal] = true;
		}

		public virtual bool SupportsIndex()
		{
			return true;
		}
	}
}
