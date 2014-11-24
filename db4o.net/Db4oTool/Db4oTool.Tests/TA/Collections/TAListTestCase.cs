/* Copyright (C) 2004 - 2010 Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using Db4objects.Db4o.Collections;
using Db4oTool.Core;

namespace Db4oTool.Tests.TA.Collections
{
	class TAListTestCase : TACollectionsTestCaseBase
	{
		public void TestMethodWithInterfaceParameter()
		{
			AssertConstructorInstrumentation("InitInterface");
			AssertConstructorInstrumentation("CollectionInitInterface");
		}

		public void TestEnumerableAsArgumentToParams()
		{
			AssertConstructorInstrumentation("Create", typeof(IEnumerable<>));
			AssertConstructorInstrumentation("Create", typeof(IEnumerable<string>));
		}

		public void TestLocalsAsInterface()
		{
			AssertConstructorInstrumentation("LocalsAsIList");
			AssertConstructorInstrumentation("CollectionLocalsAsIList");
		}

		public void TestMethodReturningNewListAsInterface()
		{
			AssertConstructorInstrumentation("CreateList");
			AssertConstructorInstrumentation("CollectionCreateList");
		}

		public void TestAssignmentOfConstructorLessListToInterface()
		{
			AssertConstructorInstrumentation("ParameterLessConstructor");
			AssertConstructorInstrumentation("CollectionParameterLessConstructor");
		}

		public void TestConstructorsWarnings()
		{
			AssertConstructorInstrumentationWarning("InitConcrete");
			AssertConstructorInstrumentationWarning("AssignmentOfConcreteListToLocal");
			AssertConstructorInstrumentationWarning("AssignmentOfConcreteListToField");
			AssertConstructorInstrumentationWarning("PublicCreateConcreteList");
		}

		public void TestSuccessfulCasts()
		{
			AssertSuccessfulCast("CastFollowedByParameterLessMethod");
			AssertSuccessfulCast("CastFollowedByMethodWithSingleArgument");
			AssertSuccessfulCast("CastConsumedByPropertyAccess");
		}

		public void TestFailingCasts()
		{
			AssertFailingCast("CastConsumedByLocal");
			AssertFailingCast("CastConsumedByField");
			AssertFailingCast("CastConsumedByArgument");
			AssertFailingCast("CastConsumedByMethodReturn");
		}

		protected override Configuration Configuration(string assemblyLocation)
        {
            Configuration conf = base.Configuration(assemblyLocation);
            conf.PreserveDebugInfo = true;
            return conf;
        }

		#region Overrides of TACollectionsTestCaseBase

		protected override string TestResource
		{
			get { return "TACollectionsScenarios"; }
		}

		protected override Type ReplacementType
		{
			get { return typeof(ActivatableList<string>); }
		}

		protected override Type OriginalType
		{
			get { return typeof(List<string>); }
		}

		#endregion
	}
}
