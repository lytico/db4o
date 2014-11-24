/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class UntypedEvaluationTestCase : AbstractDb4oTestCase
	{
		private static readonly Type Extent = typeof(object);

		public class Data
		{
			public int _id;

			public Data(int id)
			{
				// replace with Data.class -> green
				_id = id;
			}
		}

		[System.Serializable]
		public class UntypedEvaluation : IEvaluation
		{
			public bool _value;

			public UntypedEvaluation(bool value)
			{
				_value = value;
			}

			public virtual void Evaluate(ICandidate candidate)
			{
				candidate.Include(_value);
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new UntypedEvaluationTestCase.Data(42));
		}

		public virtual void TestUntypedRaw()
		{
			IQuery query = NewQuery(Extent);
			Assert.AreEqual(1, query.Execute().Count);
		}

		public virtual void TestUntypedEvaluationNone()
		{
			IQuery query = NewQuery(Extent);
			query.Constrain(new UntypedEvaluationTestCase.UntypedEvaluation(false));
			Assert.AreEqual(0, query.Execute().Count);
		}

		public virtual void TestUntypedEvaluationAll()
		{
			IQuery query = NewQuery(Extent);
			query.Constrain(new UntypedEvaluationTestCase.UntypedEvaluation(true));
			Assert.AreEqual(1, query.Execute().Count);
		}
	}
}
