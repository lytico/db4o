/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Sampledata;
using Db4objects.Db4o.Tests.Common.Types.Arrays;

namespace Db4objects.Db4o.Tests.Common.Types.Arrays
{
	public class TypedArrayInObjectTestCase : AbstractDb4oTestCase
	{
		private static readonly AtomData[] Array = new AtomData[] { new AtomData("TypedArrayInObject"
			) };

		public class Data
		{
			public object _obj;

			public object[] _objArr;

			public Data(object obj, object[] obj2)
			{
				this._obj = obj;
				this._objArr = obj2;
			}
		}

		protected override void Store()
		{
			TypedArrayInObjectTestCase.Data data = new TypedArrayInObjectTestCase.Data(Array, 
				Array);
			Db().Store(data);
		}

		public virtual void TestRetrieve()
		{
			TypedArrayInObjectTestCase.Data data = (TypedArrayInObjectTestCase.Data)((TypedArrayInObjectTestCase.Data
				)RetrieveOnlyInstance(typeof(TypedArrayInObjectTestCase.Data)));
			Assert.IsTrue(data._obj is AtomData[], "Expected instance of " + typeof(AtomData[]
				) + ", but got " + data._obj);
			Assert.IsTrue(data._objArr is AtomData[], "Expected instance of " + typeof(AtomData
				[]) + ", but got " + data._objArr);
			ArrayAssert.AreEqual(Array, data._objArr);
			ArrayAssert.AreEqual(Array, (AtomData[])data._obj);
		}
	}
}
