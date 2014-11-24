/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Tests.CLI1.NativeQueries
{
	public class NativeQueriesTestCase : AbstractNativeQueriesTestCase
	{	
#if !SILVERLIGHT
		private Data _a;
		private Data _b;
		private Data _c1;
		private Data _c2;

        protected override void Db4oSetupAfterStore()
        {
            _a = new Data(1, "Aa", null, DateTime.Today.AddDays(5), Priority.High);
            _b = new Data(2, "Bb", _a, DateTime.Today.AddDays(10), Priority.High);
            _c1 = new Data(3 ,"Cc",_b, DateTime.Today.AddDays(15), Priority.Low);
            _c2 = new Data(3, "Cc", null, DateTime.Today.AddDays(20), Priority.Low);

			Store(_a);
			Store(_b);
			Store(_c1);
			Store(_c2);
		}
	    
	    public void TestReturnTrue()
	    {
	        AssertNQResult(new ReturnTruePredicate(), _a, _b, _c1, _c2);
	    }

		public void TestDescendRightSideField()
		{
			AssertNQResult(new DescendRightSideField(), _b);
		}

		public void TestRightSideField()
		{
			AssertNQResult(new RightSideField(), _b);
		}

		public void TestEnumPredicate()
		{	
			AssertNQResult(new WithPriority(Priority.Low), _c1, _c2);
			AssertNQResult(new WithPriority(Priority.Normal));
            AssertNQResult(new WithPriority(Priority.High), _a, _b);
            AssertNQResult(new WithPriority(Priority.None));
        }

		public void TestDateRange()
		{
			AssertNQResult(new DateRange(DateTime.Now, DateTime.Now.AddDays(1)));
			AssertNQResult(new DateRange(DateTime.Now, DateTime.Now.AddDays(6)), _a);
			AssertNQResult(new DateRange(DateTime.Now.AddDays(4), DateTime.Now.AddDays(6)), _a);
			AssertNQResult(new DateRange(DateTime.Now.AddDays(4), DateTime.Now.AddDays(14)), _a, _b);
			AssertNQResult(new DateRange(DateTime.Now.AddDays(4), DateTime.Now.AddDays(30)), _a, _b, _c1, _c2);
			AssertNQResult(new DateRange(DateTime.Now.AddDays(14), DateTime.Now.AddDays(21)), _c1, _c2);
			AssertNQResult(new DateRange(DateTime.Now.AddDays(19), DateTime.Now.AddDays(30)), _c2);
		}

		public void TestNestedAnd()
		{
			AssertNQResult(new NestedAnd(), _c1);
		}

		public void TestIdDisjunction()
		{
			AssertNQResult(new IdDisjunction(), _a, _c1, _c2);
		}

		public void TestNestedOr() 
		{
			AssertNQResult(new NestedOr(), _a, _b);
		}

		public void TestHasPreviousWithPrevious()
		{
			AssertNQResult(new HasPreviousWithPrevious(), _c1);
		}

		public void TestFieldGetterHasPreviousWithName()
		{
			AssertNQResult(new FieldGetterHasPreviousWithName("Aa"), _b);
			AssertNQResult(new FieldGetterHasPreviousWithName("Bb"), _c1);
			AssertNQResult(new FieldGetterHasPreviousWithName("Cc"));
		}

		public void TestGetterGetterHasPreviousWithName()
		{
			AssertNQResult(new GetterGetterHasPreviousWithName("Aa"), _b);
			AssertNQResult(new GetterGetterHasPreviousWithName("Bb"), _c1);
			AssertNQResult(new GetterGetterHasPreviousWithName("Cc"));
		}

		public void TestGetterHasPreviousWithName()
		{
			AssertNQResult(new GetterHasPreviousWithName("Aa"), _b);
			AssertNQResult(new GetterHasPreviousWithName("Bb"), _c1);
			AssertNQResult(new GetterHasPreviousWithName("Cc"));
		}

		public void TestIdGreaterAndNameEqual()
		{
			AssertNQResult(new IdGreaterAndNameEqual(), _c1, _c2);
		}

		public void TestIdRange()
		{
			AssertNQResult(new IdValidRange(), _b);
			AssertNQResult(new IdInvalidRange());
			AssertNQResult(new IdRange(1, 2), _a, _b);
			AssertNQResult(new IdRange(1, 3), _a, _b, _c1, _c2);
			AssertNQResult(new IdRange(1, 1), _a);
			AssertNQResult(new IdRange(3, 4), _c1, _c2);
			AssertNQResult(new IdRange(0, 1), _a);
			AssertNQResult(new IdRange(4, 5));
			AssertNQResult(new IdRange(-1, 0));
		}

		public void TestPreviousIdGreaterOrEqual()
		{
			AssertNQResult(new PreviousIdGreaterOrEqual(1), _b, _c1);
			AssertNQResult(new PreviousIdGreaterOrEqual(2), _c1);
			AssertNQResult(new PreviousIdGreaterOrEqual(3));
		}

		public void TestHasPreviousWithName()
		{
			AssertNQResult(new HasPreviousWithName("Aa"), _b);
			AssertNQResult(new HasPreviousWithName("Bb"), _c1);
			AssertNQResult(new HasPreviousWithName("Cc"));
		}

		public void TestPredicateFieldIdGreaterOrEqualThan()
		{
			AssertNQResult(new IdGreaterOrEqualThan(2), _b, _c1, _c2);
			AssertNQResult(new IdGreaterOrEqualThan(3), _c1, _c2);
			AssertNQResult(new IdGreaterOrEqualThan(4));
			AssertNQResult(new IdGreaterOrEqualThan(1), _a, _b, _c1, _c2);
		}

		public void TestPredicateFieldNameEqualsTo()
		{
			AssertNQResult(new NameEqualsTo("Bb"), _b);
			AssertNQResult(new NameEqualsTo("Cc"), _c1, _c2);
			AssertNQResult(new NameEqualsTo("None"));
		}

		public void TestPredicateFieldNameOrId()
		{
			AssertNQResult(new NameOrId("Bb", 3), _b, _c1, _c2);
			AssertNQResult(new NameOrId("Aa", 2), _a, _b);
			AssertNQResult(new NameOrId("None", -1));
		}

		public void TestNotIntFieldEqual()
		{
			AssertNQResult(new NotIntFieldEqual(), _b, _c1, _c2);
		}

		public void TestNotIntGetterGreater()
		{
			AssertNQResult(new NotIntGetterGreater(), _a, _b);
		}

		public void TestNotStringGetterEqual()
		{
			AssertNQResult(new NotStringGetterEqual(), _a, _b);
		}

		public void TestCandidateNestedMethodInvocation()
		{
			AssertNQResult(new CandidateNestedMethodInvocation(), _b, _c1);
		}
	
		public void TestIntGetterEqual()
		{
			AssertNQResult(new IntGetterEqual(), _b);
		}

		public void TestIntGetterLessThan()
		{
			AssertNQResult(new IntGetterLessThan(), _a);
		}

		public void TestIntGetterLessThanOrEqual()
		{
			AssertNQResult(new IntGetterLessThanOrEqual(), _a, _b);
		}

		public void TestIntGetterGreaterThan()
		{
			AssertNQResult(new IntGetterGreaterThan(), _c1, _c2);
		}

		public void TestIntGetterGreaterThanOrEqual()
		{
			AssertNQResult(new IntGetterGreaterThanOrEqual(), _b, _c1, _c2);
		}

		public void TestStringGetterEqual()
		{
			AssertNQResult(new StringGetterEqual(), _c1, _c2);
		}

		public void TestConstStringField()
		{
			AssertNQResult(new ConstStringFieldPredicate(), _c1, _c2);
			AssertNQResult(new ConstStringFieldPredicateEmpty());
		}

		public void TestConstStringFieldOr()
		{
			AssertNQResult(new ConstStringFieldOrPredicate(), _a, _b);
			AssertNQResult(new ConstStringFieldOrPredicateEmpty());
		}

		public void TestConstIntField()
		{
			AssertNQResult(new ConstIntFieldPredicate1(), _a);
			AssertNQResult(new ConstIntFieldPredicate2(), _b);
		}

		public void TestConstIntFieldOr()
		{
			AssertNQResult(new ConstIntFieldOrPredicate(), _a, _b);
		}

		public void TestIntFieldLessThanConst()
		{
			AssertNQResult(new IntFieldLessThanConst(), _a);
		}

		public void TestIntFieldGreaterThanConst()
		{
			AssertNQResult(new IntFieldGreaterThanConst(), _c1, _c2);
		}

		public void TestIntFieldLessThanOrEqualConst()
		{
			AssertNQResult(new IntFieldLessThanOrEqualConst(), _a, _b);
		}

		public void TestIntFieldGreaterThanOrEqualConst()
		{
			AssertNQResult(new IntFieldGreaterThanOrEqualConst(), _b, _c1, _c2);
		}

		public void TestIntGetterNotEqual()
		{
			AssertNQResult(new IntGetterNotEqual(), _a, _c1, _c2);
		}

		public void TestConstStringFieldNotEqual()
		{
			AssertNQResult(new ConstStringFieldNotEqual(), _a, _b);
		}

        /**
         * Exception in Cecil FlowAnalysis. COR-498
        */
        public void _TestIdentityComparison()
        {
            AssertNQResult(new Identity(_a));
            
        }
#endif
	}
}