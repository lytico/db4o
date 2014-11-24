/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;
using Db4objects.Db4o.Tests.Common.References;

namespace Db4objects.Db4o.Tests.Common.References
{
	public class ReferenceSystemIntegrationTestCase : AbstractDb4oTestCase
	{
		private static readonly int[] Ids = new int[] { 100, 134, 689, 666, 775 };

		private static readonly object[] References = CreateReferences();

		public static void Main(string[] args)
		{
			new ReferenceSystemIntegrationTestCase().RunSolo();
		}

		public virtual void TestTransactionalReferenceSystem()
		{
			IReferenceSystem transactionalReferenceSystem = new TransactionalReferenceSystem(
				);
			AssertAllRerefencesAvailableOnNew(transactionalReferenceSystem);
			transactionalReferenceSystem.Rollback();
			AssertEmpty(transactionalReferenceSystem);
			AssertAllRerefencesAvailableOnCommit(transactionalReferenceSystem);
		}

		public virtual void TestHashCodeReferenceSystem()
		{
			IReferenceSystem referenceSystem = new HashcodeReferenceSystem();
			AssertAllRerefencesAvailableOnNew(referenceSystem);
		}

		private void AssertAllRerefencesAvailableOnCommit(IReferenceSystem referenceSystem
			)
		{
			FillReferenceSystem(referenceSystem);
			referenceSystem.Commit();
			AssertAllReferencesAvailable(referenceSystem);
		}

		private void AssertAllRerefencesAvailableOnNew(IReferenceSystem referenceSystem)
		{
			FillReferenceSystem(referenceSystem);
			AssertAllReferencesAvailable(referenceSystem);
		}

		private void AssertEmpty(IReferenceSystem referenceSystem)
		{
			AssertContains(referenceSystem, new object[] {  });
		}

		private void AssertAllReferencesAvailable(IReferenceSystem referenceSystem)
		{
			AssertContains(referenceSystem, References);
		}

		private void AssertContains(IReferenceSystem referenceSystem, object[] objects)
		{
			ExpectingVisitor expectingVisitor = new ExpectingVisitor(objects);
			referenceSystem.TraverseReferences(expectingVisitor);
			expectingVisitor.AssertExpectations();
		}

		private void FillReferenceSystem(IReferenceSystem referenceSystem)
		{
			for (int i = 0; i < References.Length; i++)
			{
				referenceSystem.AddNewReference((ObjectReference)References[i]);
			}
		}

		private static object[] CreateReferences()
		{
			object[] references = new object[Ids.Length];
			for (int i = 0; i < Ids.Length; i++)
			{
				ObjectReference @ref = new ObjectReference(Ids[i]);
				@ref.SetObject(Ids[i].ToString());
				references[i] = @ref;
			}
			return references;
		}
	}
}
