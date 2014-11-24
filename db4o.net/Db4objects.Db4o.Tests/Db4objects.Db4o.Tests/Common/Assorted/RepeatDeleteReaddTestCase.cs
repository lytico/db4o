/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Text;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class RepeatDeleteReaddTestCase : Db4oTestWithTempFile
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(RepeatDeleteReaddTestCase)).Run();
		}

		public class ItemA
		{
			public int _id;

			public ItemA(int id)
			{
				_id = id;
			}

			public override string ToString()
			{
				return "A" + _id;
			}
		}

		public class ItemB
		{
			public string _name;

			public ItemB(string name)
			{
				_name = name;
			}

			public override string ToString()
			{
				return "A" + _name;
			}
		}

		private const int NumItemsPerClass = 10;

		private const int DeleteRatio = 3;

		private int NumRuns = 10;

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Test()
		{
			for (int idx = 0; idx < NumRuns; idx++)
			{
				AssertRun();
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void AssertRun()
		{
			string fileName = TempFile();
			new Sharpen.IO.File(fileName).Delete();
			CreateDatabase(fileName);
			AssertCanRead(fileName);
			new Sharpen.IO.File(fileName).Delete();
		}

		private void CreateDatabase(string fileName)
		{
			IObjectContainer db = Db4oEmbedded.OpenFile(Config(), fileName);
			Collection4 removed = new Collection4();
			for (int idx = 0; idx < NumItemsPerClass; idx++)
			{
				RepeatDeleteReaddTestCase.ItemA itemA = new RepeatDeleteReaddTestCase.ItemA(idx);
				RepeatDeleteReaddTestCase.ItemB itemB = new RepeatDeleteReaddTestCase.ItemB(FillStr
					('x', idx));
				db.Store(itemA);
				db.Store(itemB);
				if ((idx % DeleteRatio) == 0)
				{
					removed.Add(itemA);
					removed.Add(itemB);
				}
			}
			db.Commit();
			DeleteAndReadd(db, removed);
			db.Close();
		}

		private void DeleteAndReadd(IObjectContainer db, Collection4 removed)
		{
			IEnumerator removeIter = removed.GetEnumerator();
			while (removeIter.MoveNext())
			{
				object cur = removeIter.Current;
				db.Delete(cur);
			}
			db.Commit();
			IEnumerator readdIter = removed.GetEnumerator();
			while (readdIter.MoveNext())
			{
				object cur = readdIter.Current;
				db.Store(cur);
			}
			db.Commit();
		}

		private void AssertCanRead(string fileName)
		{
			IObjectContainer db = Db4oEmbedded.OpenFile(Config(), fileName);
			AssertResults(db);
			db.Close();
		}

		private void AssertResults(IObjectContainer db)
		{
			AssertResult(db, typeof(RepeatDeleteReaddTestCase.ItemA));
			AssertResult(db, typeof(RepeatDeleteReaddTestCase.ItemB));
		}

		private void AssertResult(IObjectContainer db, Type clazz)
		{
			IObjectSet result = db.Query(clazz);
			Assert.AreEqual(NumItemsPerClass, result.Count);
			while (result.HasNext())
			{
				Assert.IsInstanceOf(clazz, result.Next());
			}
		}

		private IEmbeddedConfiguration Config()
		{
			IEmbeddedConfiguration config = NewConfiguration();
			config.Common.ReflectWith(Platform4.ReflectorForType(typeof(RepeatDeleteReaddTestCase.ItemA
				)));
			return config;
		}

		private string FillStr(char ch, int len)
		{
			StringBuilder buf = new StringBuilder();
			for (int idx = 0; idx < len; idx++)
			{
				buf.Append(ch);
			}
			return buf.ToString();
		}
	}
}
