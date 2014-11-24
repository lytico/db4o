/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public class QENot : QE
	{
		private QE i_evaluator;

		public QENot()
		{
		}

		internal QENot(QE a_evaluator)
		{
			// CS
			i_evaluator = a_evaluator;
		}

		internal override QE Add(QE evaluator)
		{
			if (!(evaluator is Db4objects.Db4o.Internal.Query.Processor.QENot))
			{
				i_evaluator = i_evaluator.Add(evaluator);
			}
			return this;
		}

		public virtual QE Evaluator()
		{
			return i_evaluator;
		}

		public override bool Identity()
		{
			return i_evaluator.Identity();
		}

		internal override bool IsDefault()
		{
			return false;
		}

		internal override bool Evaluate(QConObject a_constraint, IInternalCandidate a_candidate
			, object a_value)
		{
			return !i_evaluator.Evaluate(a_constraint, a_candidate, a_value);
		}

		internal override bool Not(bool res)
		{
			return !res;
		}

		public override void IndexBitMap(bool[] bits)
		{
			i_evaluator.IndexBitMap(bits);
			for (int i = 0; i < 4; i++)
			{
				bits[i] = !bits[i];
			}
		}

		public override bool SupportsIndex()
		{
			return i_evaluator.SupportsIndex();
		}
	}
}
