/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;
using Db4objects.Db4o.Tests.Common.References;

namespace Db4objects.Db4o.Tests.Common.References
{
	public abstract class ReferenceSystemTestCaseBase : ITestLifeCycle
	{
		private class Data
		{
		}

		private IReferenceSystem _refSys;

		public virtual void TestEmpty()
		{
			AssertNullReference(42, new ReferenceSystemTestCaseBase.Data());
		}

		public virtual void TestAddDeleteReaddOne()
		{
			int id = 42;
			ReferenceSystemTestCaseBase.Data data = new ReferenceSystemTestCaseBase.Data();
			ObjectReference @ref = CreateRef(id, data);
			_refSys.AddNewReference(@ref);
			AssertReference(id, data, @ref);
			_refSys.RemoveReference(@ref);
			AssertNullReference(id, data);
			_refSys.AddNewReference(@ref);
			AssertReference(id, data, @ref);
		}

		public virtual void TestDanglingReferencesAreRemoved()
		{
			int[] id = new int[] { 42, 43 };
			ReferenceSystemTestCaseBase.Data[] data = new ReferenceSystemTestCaseBase.Data[] 
				{ new ReferenceSystemTestCaseBase.Data(), new ReferenceSystemTestCaseBase.Data()
				 };
			ObjectReference ref0 = CreateRef(id[0], data[0]);
			ObjectReference ref1 = CreateRef(id[1], data[1]);
			_refSys.AddNewReference(ref0);
			_refSys.AddNewReference(ref1);
			_refSys.RemoveReference(ref0);
			_refSys.RemoveReference(ref1);
			_refSys.AddNewReference(ref0);
			AssertReference(id[0], data[0], ref0);
			AssertNullReference(id[1], data[1]);
		}

		private void AssertNullReference(int id, ReferenceSystemTestCaseBase.Data data)
		{
			Assert.IsNull(_refSys.ReferenceForId(id));
			Assert.IsNull(_refSys.ReferenceForObject(data));
		}

		private void AssertReference(int id, ReferenceSystemTestCaseBase.Data data, ObjectReference
			 @ref)
		{
			Assert.AreSame(@ref, _refSys.ReferenceForId(id));
			Assert.AreSame(@ref, _refSys.ReferenceForObject(data));
		}

		private ObjectReference CreateRef(int id, ReferenceSystemTestCaseBase.Data data)
		{
			ObjectReference @ref = new ObjectReference(id);
			@ref.SetObject(data);
			return @ref;
		}

		public virtual void SetUp()
		{
			_refSys = CreateReferenceSystem();
		}

		public virtual void TearDown()
		{
		}

		protected abstract IReferenceSystem CreateReferenceSystem();
	}
}
