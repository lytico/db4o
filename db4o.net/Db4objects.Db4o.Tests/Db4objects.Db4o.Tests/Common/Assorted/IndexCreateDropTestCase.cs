/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Util;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class IndexCreateDropTestCase : AbstractDb4oTestCase, IOptOutDefragSolo
	{
		public class IndexCreateDropItem
		{
			public int _int;

			public string _string;

			public DateTime _date;

			public IndexCreateDropItem(int int_, string string_, DateTime date_)
			{
				_int = int_;
				_string = string_;
				_date = date_;
			}

			public IndexCreateDropItem(int int_, DateTime nullDate) : this(int_, int_ == 0 ? 
				null : string.Empty + int_, int_ == 0 ? nullDate : new DateTime(int_))
			{
			}
		}

		private readonly int[] Values = new int[] { 4, 7, 6, 6, 5, 4, 0, 0 };

		public static void Main(string[] arguments)
		{
			new IndexCreateDropTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			// TODO
			base.Configure(config);
		}

		protected override void Store()
		{
			for (int i = 0; i < Values.Length; i++)
			{
				Db().Store(new IndexCreateDropTestCase.IndexCreateDropItem(Values[i], NullDate())
					);
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			AssertQueryResults();
			AssertQueryResults(true);
			AssertQueryResults(false);
			AssertQueryResults(true);
		}

		/// <exception cref="System.Exception"></exception>
		private void AssertQueryResults(bool indexed)
		{
			Indexed(indexed);
			Reopen();
			AssertQueryResults();
		}

		private void Indexed(bool flag)
		{
			IObjectClass oc = Fixture().Config().ObjectClass(typeof(IndexCreateDropTestCase.IndexCreateDropItem
				));
			oc.ObjectField("_int").Indexed(flag);
			oc.ObjectField("_string").Indexed(flag);
			oc.ObjectField("_date").Indexed(flag);
		}

		protected override IQuery NewQuery()
		{
			IQuery q = base.NewQuery();
			q.Constrain(typeof(IndexCreateDropTestCase.IndexCreateDropItem));
			return q;
		}

		private void AssertQueryResults()
		{
			IQuery q = NewQuery();
			q.Descend("_int").Constrain(6);
			AssertQuerySize(2, q);
			q = NewQuery();
			q.Descend("_int").Constrain(4).Greater();
			AssertQuerySize(4, q);
			q = NewQuery();
			q.Descend("_int").Constrain(4).Greater().Equal();
			AssertQuerySize(6, q);
			q = NewQuery();
			q.Descend("_int").Constrain(7).Smaller().Equal();
			AssertQuerySize(8, q);
			q = NewQuery();
			q.Descend("_int").Constrain(7).Smaller();
			AssertQuerySize(7, q);
			q = NewQuery();
			q.Descend("_string").Constrain("6");
			AssertQuerySize(2, q);
			q = NewQuery();
			q.Descend("_string").Constrain("7");
			AssertQuerySize(1, q);
			q = NewQuery();
			q.Descend("_string").Constrain("4");
			AssertQuerySize(2, q);
			q = NewQuery();
			q.Descend("_string").Constrain(null);
			AssertQuerySize(2, q);
			q = NewQuery();
			q.Descend("_date").Constrain(new DateTime(4)).Greater();
			AssertQuerySize(4, q);
			q = NewQuery();
			q.Descend("_date").Constrain(new DateTime(4)).Greater().Equal();
			AssertQuerySize(6, q);
			q = NewQuery();
			q.Descend("_date").Constrain(new DateTime(7)).Smaller().Equal();
			AssertQuerySize(PlatformInformation.IsJava() ? 6 : 8, q);
			q = NewQuery();
			q.Descend("_date").Constrain(new DateTime(7)).Smaller();
			AssertQuerySize(PlatformInformation.IsJava() ? 5 : 7, q);
			q = NewQuery();
			q.Descend("_date").Constrain(null);
			AssertQuerySize(PlatformInformation.IsJava() ? 2 : 0, q);
		}

		private void AssertQuerySize(int size, IQuery q)
		{
			Assert.AreEqual(size, q.Execute().Count);
		}

		/// <summary>
		/// java.util.Date gets translated to System.DateTime on .net which is
		/// a value type thus no null.
		/// </summary>
		/// <remarks>
		/// java.util.Date gets translated to System.DateTime on .net which is
		/// a value type thus no null.
		/// We ask the DateHandler the proper 'null' representation for the
		/// current platform.
		/// </remarks>
		private DateTime NullDate()
		{
			return (DateTime)Db().Reflector().ForClass(typeof(DateTime)).NullValue();
		}
	}
}
