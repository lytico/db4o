/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.soda;

import com.db4o.db4ounit.common.soda.classes.simple.*;
import com.db4o.db4ounit.common.soda.classes.typedhierarchy.*;
import com.db4o.db4ounit.common.soda.classes.untypedhierarchy.*;
import com.db4o.db4ounit.common.soda.joins.typed.*;
import com.db4o.db4ounit.common.soda.joins.untyped.*;
import com.db4o.db4ounit.common.soda.ordered.*;
import com.db4o.db4ounit.common.soda.wrapper.untyped.*;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	protected Class[] testCases() {
		return new Class[]{
				STOrderingTestCase.class,
				com.db4o.db4ounit.common.soda.arrays.AllTests.class,
				AndJoinOptimizationTestCase.class,
				ByteCoercionTestCase.class,
				CollectionIndexedJoinTestCase.class,
				InterfaceFieldConstraintTestCase.class,
				NullIdentityConstraintTestCase.class,
				OrderByParentFieldTestCase.class,
				OrderByWithComparableTestCase.class,
				OrderByWithNullValuesTestCase.class,
				OrderedOrConstraintTestCase.class,
				OrderFollowedByConstraintTestCase.class,
				PreserveJoinsTestCase.class,
				QueryUnknownClassTestCase.class,
				SODAClassTypeDescend.class,
				SortingNotAvailableField.class,
				SortMultipleTestCase.class,
				STBooleanTestCase.class,				
				STBooleanWUTestCase.class,
				STByteTestCase.class,
				STByteWUTestCase.class,
				STCharTestCase.class,
				STCharWUTestCase.class,
				STDoubleTestCase.class,
				STDoubleWUTestCase.class,
				STETH1TestCase.class,
				STFloatTestCase.class,
				STFloatWUTestCase.class,
				STIntegerTestCase.class,
				STIntegerWUTestCase.class,
				STLongTestCase.class,
				STLongWUTestCase.class,
				STOrTTestCase.class,
				STOrUTestCase.class,
				STOStringTestCase.class,
				STOIntegerTestCase.class,
				STOIntegerWTTestCase.class,
				STRTH1TestCase.class,
				STSDFT1TestCase.class,
				STShortTestCase.class,
				STShortWUTestCase.class,
				STStringUTestCase.class,
				STRUH1TestCase.class,
				STTH1TestCase.class,
				STUH1TestCase.class,
				TopLevelOrderExceptionTestCase.class,
				UntypedEvaluationTestCase.class,
				JointEqualsIdentityTestCase.class,
		};
	}
	
	public static void main(String[] args) {
		new AllTests().runSolo();
	}

}
