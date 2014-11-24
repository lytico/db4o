/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Regression;

namespace Db4objects.Db4o.Tests.Common.Regression
{
	/// <exclude></exclude>
	public class COR57TestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new COR57TestCase().RunSolo();
		}

		public class Base
		{
			public string name;

			public Base()
			{
			}

			public Base(string name_)
			{
				name = name_;
			}

			public override string ToString()
			{
				return GetType() + ":" + name;
			}
		}

		public class BaseExt : COR57TestCase.Base
		{
			public BaseExt()
			{
			}

			public BaseExt(string name_) : base(name_)
			{
			}
		}

		public class BaseExtExt : COR57TestCase.BaseExt
		{
			public BaseExtExt()
			{
			}

			public BaseExtExt(string name_) : base(name_)
			{
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(COR57TestCase.Base)).ObjectField("name").Indexed(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int i = 0; i < 5; i++)
			{
				string name = i.ToString();
				Db().Store(new COR57TestCase.Base(name));
				Db().Store(new COR57TestCase.BaseExt(name));
				Db().Store(new COR57TestCase.BaseExtExt(name));
			}
		}

		public virtual void TestQBE()
		{
			AssertQBE(1, new COR57TestCase.BaseExtExt("1"));
			AssertQBE(2, new COR57TestCase.BaseExt("1"));
			AssertQBE(3, new COR57TestCase.Base("1"));
		}

		public virtual void TestSODA()
		{
			AssertSODA(1, new COR57TestCase.BaseExtExt("1"));
			AssertSODA(2, new COR57TestCase.BaseExt("1"));
			AssertSODA(3, new COR57TestCase.Base("1"));
		}

		private void AssertSODA(int expectedCount, COR57TestCase.Base template)
		{
			AssertQueryResult(expectedCount, template, CreateSODA(template).Execute());
		}

		private IQuery CreateSODA(COR57TestCase.Base template)
		{
			IQuery q = NewQuery(template.GetType());
			q.Descend("name").Constrain(template.name);
			return q;
		}

		private void AssertQBE(int expectedCount, COR57TestCase.Base template)
		{
			AssertQueryResult(expectedCount, template, Db().QueryByExample(template));
		}

		private void AssertQueryResult(int expectedCount, COR57TestCase.Base expectedTemplate
			, IObjectSet result)
		{
			Assert.AreEqual(expectedCount, result.Count, SimpleName(expectedTemplate.GetType(
				)));
			while (result.HasNext())
			{
				COR57TestCase.Base actual = (COR57TestCase.Base)result.Next();
				Assert.AreEqual(expectedTemplate.name, actual.name);
				Assert.IsInstanceOf(expectedTemplate.GetType(), actual);
			}
		}

		private string SimpleName(Type c)
		{
			string name = c.FullName;
			return Sharpen.Runtime.Substring(name, name.LastIndexOf('$') + 1);
		}
	}
}
