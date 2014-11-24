/* Copyright (C) 2010 Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using Db4objects.Db4o.Collections;

namespace Db4oTool.Tests.TA.Collections
{
	class TADictionaryTestCase : TACollectionsTestCaseBase
	{
		public void TestConstructorIsReplaced()
		{
			AssertConstructorInstrumentation("ConstructorWithInicialCapacity", new Type[] {typeof(int)});
			
			// FIXME: Represent generic parameters in members
			//AssertConstructorInstrumentation("ConstructorWithDictionary", new Type[] { typeof(IDictionary<,>) });
		}

		public void TestCastIsReplaced()
		{
			AssertSuccessfulCast("CastFollowedByValuePropertyAccess");
		}

		public void TestAssignmentToConcreteType()
		{
			AssertConstructorInstrumentationWarning("InitConcrete");
		}

		#region Overrides of TACollectionsTestCaseBase

		protected override string TestResource
		{
			get { return "TAActivatableDictionaryScenarios"; }
		}

		protected override Type ReplacementType
		{
			get { return typeof(ActivatableDictionary<string, int>); }
		}

		protected override Type OriginalType
		{
			get { return typeof(Dictionary<string, int>); }
		}

		#endregion
	}
}
