/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Types.Arrays;

namespace Db4objects.Db4o.Tests.Common.Types.Arrays
{
	public class SimpleStringArrayTestCase : AbstractDb4oTestCase
	{
		private static readonly string[] Array = new string[] { "hi", "babe" };

		public class Data
		{
			public string[] _arr;

			public Data(string[] _arr)
			{
				this._arr = _arr;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Db().Store(new SimpleStringArrayTestCase.Data(Array));
		}

		public virtual void TestRetrieve()
		{
			SimpleStringArrayTestCase.Data data = (SimpleStringArrayTestCase.Data)((SimpleStringArrayTestCase.Data
				)RetrieveOnlyInstance(typeof(SimpleStringArrayTestCase.Data)));
			ArrayAssert.AreEqual(Array, data._arr);
		}
	}
}
