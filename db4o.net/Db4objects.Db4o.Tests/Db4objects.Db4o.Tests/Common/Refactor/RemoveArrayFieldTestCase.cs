/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Extensions.Util;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Refactor;

namespace Db4objects.Db4o.Tests.Common.Refactor
{
	public class RemoveArrayFieldTestCase : AbstractDb4oTestCase, IOptOutDefragSolo
	{
		public class DataBefore
		{
			public object[] array;

			public string name;

			public bool status;

			public DataBefore(string name, bool status, object[] array)
			{
				this.name = name;
				this.status = status;
				this.array = array;
			}
		}

		public class DataAfter
		{
			public string name;

			public bool status;

			public DataAfter(string name, bool status)
			{
				this.name = name;
				this.status = status;
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestRemoveArrayField()
		{
			RemoveArrayFieldTestCase.DataBefore dataA = new RemoveArrayFieldTestCase.DataBefore
				("a", true, new object[] { "X" });
			RemoveArrayFieldTestCase.DataBefore dataB = new RemoveArrayFieldTestCase.DataBefore
				("b", false, new object[0]);
			Store(dataA);
			Store(dataB);
			IObjectClass oc = Fixture().Config().ObjectClass(typeof(RemoveArrayFieldTestCase.DataBefore
				));
			// we must use ReflectPlatform here as the string must include
			// the assembly name in .net
			oc.Rename(CrossPlatformServices.FullyQualifiedName(typeof(RemoveArrayFieldTestCase.DataAfter
				)));
			Reopen();
			IQuery query = NewQuery(typeof(RemoveArrayFieldTestCase.DataAfter));
			query.Descend("name").Constrain("a");
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
			RemoveArrayFieldTestCase.DataAfter data = (RemoveArrayFieldTestCase.DataAfter)result
				.Next();
			Assert.AreEqual(dataA.name, data.name);
			Assert.AreEqual(dataA.status, data.status);
		}
	}
}
