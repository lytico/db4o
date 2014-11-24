/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class AllTests : ComposibleTestSuite
	{
		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(ActivationExceptionBubblesUpTestCase), typeof(
				BackupCSExceptionTestCase), typeof(DatabaseClosedExceptionTestCase), typeof(DatabaseReadonlyExceptionTestCase
				), typeof(GlobalOnlyConfigExceptionTestCase), typeof(ObjectCanActiviateExceptionTestCase
				), typeof(ObjectCanDeleteExceptionTestCase), typeof(ObjectOnDeleteExceptionTestCase
				), typeof(ObjectCanNewExceptionTestCase), typeof(StoreExceptionBubblesUpTestCase
				), typeof(StoredClassExceptionBubblesUpTestCase), typeof(TSerializableOnInstantiateCNFExceptionTestCase
				), typeof(TSerializableOnInstantiateIOExceptionTestCase), typeof(TSerializableOnStoreExceptionTestCase
				) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(BackupDb4oIOExceptionTestCase), typeof(IncompatibleFileFormatExceptionTestCase
				), typeof(InvalidSlotExceptionTestCase), typeof(OldFormatExceptionTestCase), typeof(
				Db4objects.Db4o.Tests.Common.Exceptions.Propagation.AllTests), typeof(InvalidPasswordTestCase
				) };
		}
		#endif // !SILVERLIGHT
	}
}
