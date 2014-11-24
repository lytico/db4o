/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Experiments
{
	public class STCaseInsensitiveTestCase : SodaBaseTestCase
	{
		public string str;

		public STCaseInsensitiveTestCase()
		{
		}

		public STCaseInsensitiveTestCase(string str)
		{
			this.str = str;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Experiments.STCaseInsensitiveTestCase
				("Hihoho"), new Db4objects.Db4o.Tests.Common.Soda.Experiments.STCaseInsensitiveTestCase
				("Hello"), new Db4objects.Db4o.Tests.Common.Soda.Experiments.STCaseInsensitiveTestCase
				("hello") };
		}

		public virtual void Test()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Experiments.STCaseInsensitiveTestCase
				));
			q.Descend("str").Constrain(new _IEvaluation_30());
			Expect(q, new int[] { 1, 2 });
		}

		private sealed class _IEvaluation_30 : IEvaluation
		{
			public _IEvaluation_30()
			{
			}

			public void Evaluate(ICandidate candidate)
			{
				candidate.Include(candidate.GetObject().ToString().ToLower().StartsWith("hell"));
			}
		}
	}
}
