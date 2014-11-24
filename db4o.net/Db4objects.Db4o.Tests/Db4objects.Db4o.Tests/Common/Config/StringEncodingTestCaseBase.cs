/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	/// <exclude></exclude>
	public abstract class StringEncodingTestCaseBase : AbstractDb4oTestCase
	{
		public class Item
		{
			public Item(string name)
			{
				_name = name;
			}

			public string _name;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestStoreSimpleObject()
		{
			string name = "one";
			Store(new StringEncodingTestCaseBase.Item(name));
			Reopen();
			StringEncodingTestCaseBase.Item item = (StringEncodingTestCaseBase.Item)((StringEncodingTestCaseBase.Item
				)RetrieveOnlyInstance(typeof(StringEncodingTestCaseBase.Item)));
			Assert.AreEqual(name, item._name);
		}

		public virtual void TestCorrectStringIoClass()
		{
			Assert.AreSame(StringIoClass(), Container().StringIO().GetType());
		}

		protected abstract Type StringIoClass();
	}
}
