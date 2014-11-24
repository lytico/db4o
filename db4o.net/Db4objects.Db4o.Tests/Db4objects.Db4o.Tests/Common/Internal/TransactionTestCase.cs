/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class TransactionTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		private const int TestId = 5;

		public virtual void TestRemoveReferenceSystemOnClose()
		{
			LocalObjectContainer container = (LocalObjectContainer)Db();
			IReferenceSystem referenceSystem = container.CreateReferenceSystem();
			Transaction transaction = container.NewTransaction(container.SystemTransaction(), 
				referenceSystem, false);
			referenceSystem.AddNewReference(new ObjectReference(TestId));
			referenceSystem.AddNewReference(new ObjectReference(TestId + 1));
			container.ReferenceSystemRegistry().RemoveId(TestId);
			Assert.IsNull(referenceSystem.ReferenceForId(TestId));
			transaction.Close(false);
			container.ReferenceSystemRegistry().RemoveId(TestId + 1);
			Assert.IsNotNull(referenceSystem.ReferenceForId(TestId + 1));
		}
	}
}
