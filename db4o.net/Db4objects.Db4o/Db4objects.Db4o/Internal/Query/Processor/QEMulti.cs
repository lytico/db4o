/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public class QEMulti : QE
	{
		private Collection4 i_evaluators = new Collection4();

		// used by .net LINQ tests
		public virtual IEnumerable Evaluators()
		{
			return i_evaluators;
		}

		internal override QE Add(QE evaluator)
		{
			i_evaluators.Ensure(evaluator);
			return this;
		}

		public override bool Identity()
		{
			bool ret = false;
			IEnumerator i = i_evaluators.GetEnumerator();
			while (i.MoveNext())
			{
				if (((QE)i.Current).Identity())
				{
					ret = true;
				}
				else
				{
					return false;
				}
			}
			return ret;
		}

		internal override bool IsDefault()
		{
			return false;
		}

		internal override bool Evaluate(QConObject a_constraint, IInternalCandidate a_candidate
			, object a_value)
		{
			IEnumerator i = i_evaluators.GetEnumerator();
			while (i.MoveNext())
			{
				if (((QE)i.Current).Evaluate(a_constraint, a_candidate, a_value))
				{
					return true;
				}
			}
			return false;
		}

		public override void IndexBitMap(bool[] bits)
		{
			IEnumerator i = i_evaluators.GetEnumerator();
			while (i.MoveNext())
			{
				((QE)i.Current).IndexBitMap(bits);
			}
		}

		public override bool SupportsIndex()
		{
			IEnumerator i = i_evaluators.GetEnumerator();
			while (i.MoveNext())
			{
				if (!((QE)i.Current).SupportsIndex())
				{
					return false;
				}
			}
			return true;
		}
	}
}
