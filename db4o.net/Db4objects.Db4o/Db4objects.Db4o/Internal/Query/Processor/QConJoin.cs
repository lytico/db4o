/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <summary>Join constraint on queries</summary>
	/// <exclude></exclude>
	public class QConJoin : QCon
	{
		private bool i_and;

		private QCon i_constraint1;

		private QCon i_constraint2;

		public QConJoin()
		{
		}

		internal QConJoin(Transaction a_trans, QCon a_c1, QCon a_c2, bool a_and) : base(a_trans
			)
		{
			// FIELDS MUST BE PUBLIC TO BE REFLECTED ON UNDER JDK <= 1.1
			// C/S
			i_constraint1 = a_c1;
			i_constraint2 = a_c2;
			i_and = a_and;
		}

		public virtual QCon Constraint2()
		{
			return i_constraint2;
		}

		public virtual QCon Constraint1()
		{
			return i_constraint1;
		}

		internal override void DoNotInclude(IInternalCandidate root)
		{
			Constraint1().DoNotInclude(root);
			Constraint2().DoNotInclude(root);
		}

		internal override void ExchangeConstraint(QCon a_exchange, QCon a_with)
		{
			base.ExchangeConstraint(a_exchange, a_with);
			if (a_exchange == Constraint1())
			{
				i_constraint1 = a_with;
			}
			if (a_exchange == Constraint2())
			{
				i_constraint2 = a_with;
			}
		}

		internal virtual void EvaluatePending(IInternalCandidate root, QPending pending, 
			int secondResult)
		{
			bool res = i_evaluator.Not(i_and ? ((pending._result + secondResult) > 0) : (pending
				._result + secondResult) > QPending.False);
			if (HasJoins())
			{
				IEnumerator i = IterateJoins();
				while (i.MoveNext())
				{
					Db4objects.Db4o.Internal.Query.Processor.QConJoin qcj = (Db4objects.Db4o.Internal.Query.Processor.QConJoin
						)i.Current;
					root.Evaluate(new QPending(qcj, this, res));
				}
			}
			else
			{
				if (!res)
				{
					Constraint1().DoNotInclude(root);
					Constraint2().DoNotInclude(root);
				}
			}
		}

		public virtual QCon GetOtherConstraint(QCon a_constraint)
		{
			if (a_constraint == Constraint1())
			{
				return Constraint2();
			}
			else
			{
				if (a_constraint == Constraint2())
				{
					return Constraint1();
				}
			}
			throw new ArgumentException();
		}

		internal override string LogObject()
		{
			return string.Empty;
		}

		public override string ToString()
		{
			string str = "QConJoin " + (i_and ? "AND " : "OR");
			if (Constraint1() != null)
			{
				str += "\n   " + Constraint1();
			}
			if (Constraint2() != null)
			{
				str += "\n   " + Constraint2();
			}
			return str;
		}

		public virtual bool IsOr()
		{
			return !i_and;
		}

		public override void SetProcessedByIndex(QCandidates candidates)
		{
			if (ProcessedByIndex())
			{
				return;
			}
			base.SetProcessedByIndex(candidates);
			Constraint1().SetProcessedByIndex(candidates);
			Constraint2().SetProcessedByIndex(candidates);
		}

		protected override bool CanResolveByFieldIndex()
		{
			return false;
		}
	}
}
