/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;
using Db4objects.Db4o.Tests.Common.Soda.Classes.Simple;
using Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy;
using Db4objects.Db4o.Tests.Common.Soda.Util;
using Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Soda.Experiments
{
	public class STMagicTestCase : SodaBaseTestCase, ISTInterface
	{
		public string str;

		public STMagicTestCase()
		{
		}

		private STMagicTestCase(string str)
		{
			// JDK 1.4.x only
			// import java.util.regex.*;
			// dependant on the previous run of some other test classes
			this.str = str;
		}

		public override string ToString()
		{
			return "STMagicTestCase: " + str;
		}

		/// <summary>needed for STInterface test</summary>
		public virtual object ReturnSomething()
		{
			return str;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Experiments.STMagicTestCase
				("aaa"), new Db4objects.Db4o.Tests.Common.Soda.Experiments.STMagicTestCase("aaax"
				) };
		}

		/// <summary>
		/// Magic:
		/// Query for all objects with a known attribute,
		/// independant of the class or even if you don't
		/// know the class.
		/// </summary>
		/// <remarks>
		/// Magic:
		/// Query for all objects with a known attribute,
		/// independant of the class or even if you don't
		/// know the class.
		/// </remarks>
		public virtual void TestUnconstrainedClass()
		{
			IQuery q = NewQuery();
			q.Descend("str").Constrain("aaa");
			SodaTestUtil.Expect(q, new object[] { new Db4objects.Db4o.Tests.Common.Soda.Experiments.STMagicTestCase
				("aaa"), new STStringTestCase("aaa"), new STStringUTestCase("aaa") });
		}

		/// <summary>
		/// Magic:
		/// Query for multiple classes.
		/// </summary>
		/// <remarks>
		/// Magic:
		/// Query for multiple classes.
		/// Every class gets it's own slot in the query graph.
		/// </remarks>
		public virtual void TestMultiClass()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(STDoubleTestCase)).Or(q.Constrain(typeof(STStringTestCase)));
			object[] stDoubles = new STDoubleTestCase().CreateData();
			object[] stStrings = new STStringTestCase().CreateData();
			object[] res = new object[stDoubles.Length + stStrings.Length];
			System.Array.Copy(stDoubles, 0, res, 0, stDoubles.Length);
			System.Array.Copy(stStrings, 0, res, stDoubles.Length, stStrings.Length);
			SodaTestUtil.Expect(q, res);
		}

		/// <summary>
		/// Magic:
		/// Execute any node in the query graph.
		/// </summary>
		/// <remarks>
		/// Magic:
		/// Execute any node in the query graph.
		/// The data for this example can be found in STTH1.java.
		/// </remarks>
		public virtual void TestExecuteAnyNode()
		{
			IQuery q = NewQuery();
			q.Constrain(new STTH1TestCase().CreateData()[5]);
			q = q.Descend("h2").Descend("h3");
			//	We only get one STTH3 here, because the query is
			//	constrained by the STTH2 with the "str2" member.
			SodaTestUtil.ExpectOne(q, new STTH3("str3"));
		}

		//	public void testRegularExpression() {
		//		Query q = newQuery();
		//		q.constrain(STMagicTestCase.class);
		//		Query qStr = q.descend("str");
		//		final Pattern pattern = Pattern.compile("a*x");
		//		qStr.constrain(new Evaluation() {
		//			public void evaluate(Candidate candidate) {
		//				candidate.include(pattern.matcher(((String) candidate.getObject())).matches());
		//			}
		//		});
		//		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, _array[1]);
		//	}
		/// <summary>
		/// Magic:
		/// Querying for an implemented Interface.
		/// </summary>
		/// <remarks>
		/// Magic:
		/// Querying for an implemented Interface.
		/// Using an Evaluation allows calls to the interface methods
		/// during the run of the query.s
		/// </remarks>
		public virtual void TestInterface()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(ISTInterface));
			q.Constrain(new _IEvaluation_117());
			SodaTestUtil.Expect(q, new object[] { new Db4objects.Db4o.Tests.Common.Soda.Experiments.STMagicTestCase
				("aaa"), new STStringTestCase("aaa") });
		}

		private sealed class _IEvaluation_117 : IEvaluation
		{
			public _IEvaluation_117()
			{
			}

			public void Evaluate(ICandidate candidate)
			{
				ISTInterface sti = (ISTInterface)candidate.GetObject();
				candidate.Include(sti.ReturnSomething().Equals("aaa"));
			}
		}
	}
}
