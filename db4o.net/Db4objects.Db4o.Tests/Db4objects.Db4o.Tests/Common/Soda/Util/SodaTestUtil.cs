/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Util
{
	public class SodaTestUtil
	{
		public static void ExpectOne(IQuery query, object @object)
		{
			Expect(query, new object[] { @object });
		}

		public static void ExpectNone(IQuery query)
		{
			Expect(query, null);
		}

		public static void Expect(IQuery query, object[] results)
		{
			Expect(query, results, false);
		}

		public static void ExpectOrdered(IQuery query, object[] results)
		{
			Expect(query, results, true);
		}

		public static void Expect(IQuery query, object[] results, bool ordered)
		{
			IObjectSet set = query.Execute();
			if (results == null || results.Length == 0)
			{
				if (set.Count > 0)
				{
					Assert.Fail("No content expected.");
				}
				return;
			}
			int j = 0;
			Assert.AreEqual(results.Length, set.Count);
			while (set.HasNext())
			{
				object obj = set.Next();
				bool found = false;
				if (ordered)
				{
					if (TCompare.IsEqual(results[j], obj))
					{
						results[j] = null;
						found = true;
					}
					j++;
				}
				else
				{
					for (int i = 0; i < results.Length; i++)
					{
						if (results[i] != null)
						{
							if (TCompare.IsEqual(results[i], obj))
							{
								results[i] = null;
								found = true;
								break;
							}
						}
					}
				}
				if (ordered)
				{
					Assert.IsTrue(found, "Expected '" + SafeToString(results[j - 1]) + "' but got '" 
						+ SafeToString(obj) + "' at index " + (j - 1));
				}
				else
				{
					Assert.IsTrue(found, "Object not expected: " + SafeToString(obj));
				}
			}
			for (int i = 0; i < results.Length; i++)
			{
				if (results[i] != null)
				{
					Assert.Fail("Expected object not returned: " + results[i]);
				}
			}
		}

		private static string SafeToString(object obj)
		{
			return obj != null ? obj.ToString() : string.Empty;
		}

		private SodaTestUtil()
		{
		}
	}
}
