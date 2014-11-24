/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Tests.CLI1
{
	public class AllTests : Db4oUnit.Extensions.Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new System.Type[]
				{
                    typeof(Aliases.AllTests),
					typeof(CrossPlatform.AllTests),
#if !CF && !SILVERLIGHT
					typeof(CsAppDomains),
					typeof(CsAssemblyVersionChange),
					typeof(CsImage),
					typeof(ShutdownMultipleContainer),
#endif
                    typeof(EnumTestCase),
					typeof(Events.EventRegistryTestCase),
					typeof(Foundation.AllTests),
                    typeof(Handlers.AllTests),
					typeof(Inside.AllTests),
					typeof(Internal.AllTests),
					typeof(NativeQueries.AllTests),
					typeof(Reflect.Net.AllTests),
					typeof(Soda.AllTests),
#if !SILVERLIGHT
                    typeof(CollectionBaseTestCase),
#endif
					typeof(CsCascadeDeleteToStructs),
					typeof(CsCollections),
					typeof(CsCustomTransientAttribute),
					typeof(CsDate),
					typeof(CsDelegate),
					typeof(CsDisposableTestCase),
					typeof(CsEnum),
					// typeof(CsEvaluationDelegate),  moved to Staging because it fails
#if !SILVERLIGHT
					typeof(CsMarshalByRef),
#endif
					typeof(CsType),
					typeof(StructsTestCase),
					typeof(CsStructsRegression),
					typeof(CsValueTypesTestCase),
                    typeof(CsSystemArrayTestCase),
					typeof(CultureInfoTestCase),
                    typeof(DictionaryBaseTestCase),

#if !SILVERLIGHT
					typeof(HybridDictionaryTestCase),
#endif
					typeof(ImageTestCase),
					typeof(JavaDateCompatibilityTestCase),
                    typeof(JavaSimpleChecksumCompatibilityTestCase),
					typeof(JavaUUIDCompatibilityTestCase),
					typeof(MDArrayTestCase),
#if !CF && !SILVERLIGHT
					typeof(Monitoring.AllTests),
#endif
					typeof(NonSerializedAttributeTestCase),
					typeof(ObjectSetAsListTestCase),
					typeof(ProtectedBaseConstructorTestCase),
				};
		}
	}
}
