using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4oTestRunner;

namespace NullableArraysTest
{
	public class NullableArraysTest : AbstractDb4oTesterBase
	{
		protected override void Run()
		{
			try
			{
				NullableHolder<int> intHolder = new NullableHolder<int>(new int?[] { Int32.MinValue, 0, Int32.MaxValue, null, 0xDB40 });
				Test(intHolder);
				Test(new NullableHolder<CustomValueType>(new CustomValueType?[] { new CustomValueType("Foo", 1), null, new CustomValueType("Bar", 3) }));

				TestDescending(intHolder, 0xDB40);

				File.Delete(TempFilePath());
			}
			catch (Exception ex)
			{
				_logger.LogException(ex);
			}
		}

		private void TestDescending<T>(NullableHolder<T> expected, T candidate) where T : struct
		{
			NullableHolder<T> actual = DescendingQuery(candidate);
			Compare(expected, actual);
		}

		private NullableHolder<T> DescendingQuery<T>(T candidate) where T : struct
		{
			using (IObjectContainer db = Db4oFactory.OpenFile(TempFilePath()))
			{
				_logger.LogMessage("Descending query.");
				IQuery query = db.Query();
				query.Constrain(typeof(NullableHolder<T>));

				query.Descend("_nullables").Constrain(candidate);

				IObjectSet results = query.Execute();
				_logger.LogMessage("{0} Object(s) found.", results.Count);

				return (NullableHolder<T>)results.Next();
			}
		}

		private void Test<T>(NullableHolder<T> holder) where T : struct
		{
			_logger.LogMessage("------------- Testing with type: {0} ------------- ", typeof(T).Name);
			Store(holder);

			var results = Query<T>();
			if (results.Count != 0)
			{
				Compare(holder, results[0]);
			}
			_logger.LogMessage("------------- Finished ------------- \r\n");
		}

		private void Compare<T>(NullableHolder<T> stored, NullableHolder<T> retrieved) where T : struct
		{
			if (stored != retrieved)
			{
				_logger.LogMessage("Stored and retrieved objects differ.\r\n\r\nStored : \r\n {0}\r\nRetrieved: {1}", stored.ToString(), retrieved.ToString());
			}
			else
			{
				_logger.LogMessage("Objects matches.");
			}
		}

		private IList<NullableHolder<T>> Query<T>() where T : struct
		{
			using (IObjectContainer db = Db4oFactory.OpenFile(TempFilePath()))
			{
				_logger.LogMessage("Quering.");
				IList<NullableHolder<T>> results = db.Query<NullableHolder<T>>();
				_logger.LogMessage("{0} Object(s) found.", results.Count);

				return results.ToArray();
			}
		}

		private void Store<T>(NullableHolder<T> holder) where T : struct
		{
			using (IObjectContainer db = Db4oFactory.OpenFile(TempFilePath()))
			{
				_logger.LogMessage("Storing ", holder.ToString());
				db.Store(holder);
			}
		}
	}

	struct CustomValueType
	{
		public CustomValueType(string name, int value)
		{
			_name = name;
			_value = value;
		}

		public string _name;
		public int _value;
	}
}
